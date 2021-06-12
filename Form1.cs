using DlibDotNet;
using DlibDotNet.Dnn;
using DlibDotNet.Extensions;
using DlibDotNet.ImageTransforms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace TRC_Trainer
{
    public partial class Form1 : Form
    {
        public string datapath;
        public Info info;
        public Train tt = new Train();
        public bool save = false;

        public Form1()
        {
            InitializeComponent();
            textBoxpath.Text = Application.StartupPath;
            info = new Info(textBoxinfo);
        }

        private void buttondata_Click(object sender, EventArgs e)
        {
            //选择数据集文件夹
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "请选择存放数据集的文件夹";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                datapath = folderBrowserDialog.SelectedPath;
            }
            info.AddInfo("已选定文件夹：" + datapath, 1);
            if(File.Exists(Path.Combine(datapath, "training.xml")))
            {
                info.AddInfo("已检测到training.xml。", 1);
            }
            if (File.Exists(Path.Combine(datapath, "testing.xml")))
            {
                info.AddInfo("已检测到testing.xml。", 1);
            }
        }

        private void buttontrain_Click(object sender, EventArgs e)
        {
            //单开线程训练
            if (radioButtonnet.Checked)
            {
                tt.dataDirectory = datapath;
                tt.trainiter = Convert.ToUInt32(textBoxtrainiter.Text);
                tt.testiter = Convert.ToUInt32(textBoxtestiter.Text);
                tt.lr = Convert.ToDouble(textBoxlr.Text);
                tt.epoch = Convert.ToInt32(textBoxepoch.Text);
                ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(train);
                Thread thread = new Thread(parameterizedThreadStart);
                thread.Start(tt);
            }
            else
            {
                MessageBox.Show("功能暂时无法使用。");
                ThreadStart start = new ThreadStart(pytest);
                Thread thread = new Thread(start);
                //thread.Start();
            }
        }

        private void buttonmodpath_Click(object sender, EventArgs e)
        {
            //获取保存模型文件夹
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "请选择存放训练好的模型的文件夹";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxpath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //关闭窗口
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        public class Train
        {
            public string dataDirectory;
            public uint trainiter;
            public uint testiter;
            public double lr;
            public int epoch;
        }

        public void train(object tt)
        {
            //训练函数
            save = false;
            try
            {
                string dataDirectory = (tt as Train).dataDirectory;
                uint trainiter = (tt as Train).trainiter;
                uint testiter = (tt as Train).testiter;
                IList<Matrix<RgbPixel>> imagesTrain;
                IList<Matrix<RgbPixel>> imagesTest;
                IList<IList<MModRect>> boxesTrain;
                IList<IList<MModRect>> boxesTest;
                Dlib.LoadImageDataset(Path.Combine(dataDirectory, "training.xml"), out imagesTrain, out boxesTrain);
                Dlib.LoadImageDataset(Path.Combine(dataDirectory, "testing.xml"), out imagesTest, out boxesTest);
                info.AddInfo("已载入训练图像" + imagesTrain.LongCount().ToString() + "个，共有" + boxesTrain.LongCount().ToString() + "个标注", 1);
                info.AddInfo("已载入测试图像" + imagesTest.LongCount().ToString() + "个，共有" + boxesTest.LongCount().ToString() + "个标注", 1);
                var numOverlappedIgnoredTest = 0;
                foreach (var v in boxesTest)
                    using (var overlap = new TestBoxOverlap(0.50, 0.95))
                        numOverlappedIgnoredTest += IgnoreOverlappedBoxes(v, overlap);

                var numOverlappedIgnored = 0;
                var numAdditionalIgnored = 0;

                foreach (var v in boxesTrain)
                {
                    using (var overlap = new TestBoxOverlap(0.50, 0.95))
                        numOverlappedIgnored += IgnoreOverlappedBoxes(v, overlap);
                    foreach (var bb in v)
                    {
                        if (bb.Rect.Width < 35 && bb.Rect.Height < 35)
                        {
                            if (!bb.Ignore)
                            {
                                bb.Ignore = true;
                                ++numAdditionalIgnored;
                            }
                        }
                    }
                }
                info.AddInfo($"重叠忽略数量: {numOverlappedIgnored}", 1);
                info.AddInfo($"额外忽略数量: {numAdditionalIgnored}", 1);
                info.AddInfo($"测试集重叠忽略数量: {numOverlappedIgnoredTest}", 1);


                info.AddInfo($"训练图像数量: {imagesTrain.Count()}", 1);
                info.AddInfo($"测试图像数量: {imagesTest.Count()}", 1);

                using (var options = new MModOptions(boxesTrain, 70, 30))
                {
                    options.OverlapsIgnore = new TestBoxOverlap(0.5, 0.95);
                    
                    using (var net = new LossMmod(options, 3))
                    {
                        var detectorWindows = options.DetectorWindows.ToArray();
                        using (var subnet = net.GetSubnet())
                        using (var details = subnet.GetLayerDetails())
                        {
                            details.SetNumFilters(detectorWindows.Length);

                            using (var trainer = new DnnTrainer<LossMmod>(net))
                            {
                                trainer.SetLearningRate(0.1);
                                trainer.BeVerbose();

                                trainer.SetIterationsWithoutProgressThreshold(trainiter);    //训练迭代次数
                                trainer.SetTestIterationsWithoutProgressThreshold(testiter);    //测试迭代次数

                                const string syncFilename = "model_sync";   //同步保存文件名
                                trainer.SetSynchronizationFile(syncFilename, 5 * 60);   //同步保存时间间隔


                                IEnumerable<Matrix<RgbPixel>> miniBatchSamples;
                                IEnumerable<IEnumerable<MModRect>> miniBatchLabels;
                                using (var cropper = new RandomCropper())
                                {
                                    cropper.SetSeed(0);
                                    cropper.SetChipDims(350, 350);
                                    cropper.SetMinObjectSize(69, 28);
                                    cropper.MaxRotationDegrees = 2;

                                    using (var rnd = new Rand())
                                    {
                                        // 屏幕打印参数
                                        info.AddInfo($"训练参数：\r\n{trainer}", 1);
                                        info.AddInfo($"剪枝参数：\r\n{cropper}", 1);

                                        var cnt = 1;
                                        // 训练开始，直到学习率变小
                                        while (trainer.GetLearningRate() >= (tt as Train).lr)
                                        {
                                            //训练，每n次测试评估一次
                                            if (cnt % (tt as Train).epoch != 0 || !imagesTest.Any())
                                            {
                                                cropper.Operator(87, imagesTrain, boxesTrain, out miniBatchSamples, out miniBatchLabels);
                                                foreach (var img in miniBatchSamples)
                                                    Dlib.DisturbColors(img, rnd);

                                                LossMmod.TrainOneStep(trainer, miniBatchSamples, miniBatchLabels);

                                                miniBatchSamples.DisposeElement();
                                                miniBatchLabels.DisposeElement();
                                            }
                                            else
                                            {
                                                cropper.Operator(87, imagesTest, boxesTest, out miniBatchSamples, out miniBatchLabels);
                                                foreach (var img in miniBatchSamples)
                                                    Dlib.DisturbColors(img, rnd);

                                                LossMmod.TestOneStep(trainer, miniBatchSamples, miniBatchLabels);

                                                miniBatchSamples.DisposeElement();
                                                miniBatchLabels.DisposeElement();
                                            }
                                            info.AddInfo("轮次" + cnt.ToString() + "完成。当前学习率：" + trainer.GetLearningRate() + "。", 1);
                                            ++cnt;
                                            if (save)
                                            {
                                                break;
                                            }
                                        }
                                        // 等待
                                        info.AddInfo("开始提取网络……", 1);
                                        trainer.GetNet();
                                        info.AddInfo("===========训练完成===========", 1);

                                        // 保存网络
                                        net.Clean();
                                        LossMmod.Serialize(net, textBoxpath.Text + "/model.dat");
                                        info.AddInfo("模型已保存到" + textBoxpath.Text + "/model.dat", 1);
                                        info.AddInfo($"训练参数：\r\n{trainer}", 1);
                                        info.AddInfo($"剪枝参数：\r\n{cropper}", 1);

                                        info.AddInfo($"同步文件名: {syncFilename}", 1);
                                        info.AddInfo($"===========开始评估===========", 1);
                                        info.AddInfo($"训练集图片数量: {imagesTrain.Count()}", 1);
                                        using (var _ = new TestBoxOverlap())
                                        using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTrain, boxesTrain, _, 0, options.OverlapsIgnore))
                                            info.AddInfo($"训练集评估结果: {matrix}", 1);
                                        if (checkBoxupsample.Checked)
                                        {
                                            Dlib.UpsampleImageDataset(2, imagesTrain, boxesTrain, 1800 * 1800);
                                            using (var _ = new TestBoxOverlap())
                                            using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTrain, boxesTrain, _, 0, options.OverlapsIgnore))
                                                info.AddInfo($"上采样训练集评估结果: {matrix}", 1);
                                        }

                                        info.AddInfo("测试集图片数量: {images_test.Count()}", 1);
                                        using (var _ = new TestBoxOverlap())
                                        using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTest, boxesTest, _, 0, options.OverlapsIgnore))
                                            info.AddInfo($"测试集评估结果: {matrix}", 1);
                                        if (checkBoxupsample.Checked)
                                        {
                                            Dlib.UpsampleImageDataset(2, imagesTest, boxesTest, 1800 * 1800);
                                            using (var _ = new TestBoxOverlap())
                                            using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTest, boxesTest, _, 0, options.OverlapsIgnore))
                                                info.AddInfo($"上采样测试集评估结果: {matrix}", 1);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                info.AddInfo("==========全部训练完成==========", 1);
            }
            catch (Exception ex)
            {
                info.AddInfo(ex.InnerException + "|" + ex.Source + "|" + ex.StackTrace + "|" + ex.Message, 2);
            }
        }

        private static int IgnoreOverlappedBoxes(IList<MModRect> boxes, TestBoxOverlap overlaps)
        {
            //忽略的标注
            var numIgnored = 0;
            for (var i = 0; i < boxes.Count; ++i)
            {
                if (boxes[i].Ignore)
                    continue;

                for (var j = i + 1; j < boxes.Count; ++j)
                {
                    if (boxes[j].Ignore)
                        continue;

                    if (overlaps.Operator(boxes[i], boxes[j]))
                    {
                        ++numIgnored;
                        if (boxes[i].Rect.Area < boxes[j].Rect.Area)
                            boxes[i].Ignore = true;
                        else
                            boxes[j].Ignore = true;
                    }
                }
            }

            return numIgnored;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //载入时提示
            info.AddColorInfo("请确保文件夹中含有 training.xml 和 testing.xml 两个文件。\r\n", Color.Red);
            CheckCUDA();
            ThreadStart start = new ThreadStart(testCUDA);
            Thread thread = new Thread(start);
            thread.Start();
        }

        private void buttonexit_Click(object sender, EventArgs e)
        {
            //关闭窗口
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }
        }

        public void CheckCUDA()
        {
            //检查CUDA是否可用
            int v = 0;
            bool get = DlibDotNet.Cuda.TryGetDriverVersion(out v);
            if (Dlib.IsSupportCuda)
            {
                textBoxcuda.BeginInvoke(new Action(() =>
                {
                    textBoxcuda.Text = "支持";
                    textBoxcuda.ForeColor = Color.Green;
                }));
            }
            else
            {
                textBoxcuda.BeginInvoke(new Action(() =>
                {
                    textBoxcuda.Text = "不支持";
                    textBoxcuda.ForeColor = Color.Black;
                }));
            }
        }

        private void pytest()
        {
            //调用python
            Process p = new Process();
            p.StartInfo.FileName = "python";    //填写exe的具体路径
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = Path.Combine(Application.StartupPath, "Dlib Train.py");    //参数
            p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            p.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.Close();
        }

        public void testCUDA()
        {
            //调用python
            Process p = new Process();
            p.StartInfo.FileName = "python";    //填写exe的具体路径
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "C: \\Users\\llz\\source\\repos\\TRC Trainer\\TRC Trainer\\bin\\x64\\Debug\\netcoreapp3.0\\TestCUDA.py";    //参数
            p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            p.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);
            p.Start();
            StreamReader sr = p.StandardOutput;
            while (!sr.EndOfStream)
            {
                info.AddInfo(sr.ReadLine(), 1);
            }
        }

        void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //捕捉python输出
            info.AddInfo(outLine.Data, 1);
        }

        void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //捕捉python警告
            info.AddInfo(outLine.Data, 2);
        }

        private void buttonstopsave_Click(object sender, EventArgs e)
        {
            info.AddInfo("将在本轮训练完成后停止并保存模型。", 1);
            save = true;
        }
    }

    public class Info
    {
        RichTextBox textBoxinfo = new RichTextBox();

        public Info(RichTextBox rtb)
        {
            textBoxinfo = rtb;
        }
        public void AddInfo(string info, int type)
        {
            //创建运行日志，1表示测试信息+换行，2表示警告信息+换行，3表示信息本身
            if (type == 1)
            {
                textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.AppendText("["); }));
                AddColorInfo("测试", Color.Green);
                textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.AppendText("]" + DateTime.Now + " " + info + "\r\n"); }));
            }
            else if (type == 2)
            {
                textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.AppendText("["); }));
                AddColorInfo("警告", Color.Red);
                textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.AppendText("]" + DateTime.Now + " " + info + "\r\n"); }));
            }
            else if (type == 3)
            {
                textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.AppendText(info); }));
            }
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.ScrollToCaret(); }));
        }

        public void AddColorInfo(string info, Color color)
        {
            //用于输出带颜色的信息
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.SelectionStart = textBoxinfo.TextLength; }));
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.SelectionLength = 0; }));
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.SelectionColor = color; }));
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.AppendText(info); }));
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.SelectionColor = textBoxinfo.ForeColor; }));
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.ScrollToCaret(); }));
        }
    }
}

