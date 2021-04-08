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

namespace TRC_Trainer
{
    public partial class Form1 : Form
    {
        public string datapath;
        public Form1()
        {
            InitializeComponent();
            textBoxpath.Text = Application.StartupPath;
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

        private void buttondata_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.Description = "请选择存放数据集的文件夹";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                datapath = folderBrowserDialog.SelectedPath;
            }
            AddColorInfo("请确保文件夹中含有 training.xml 和 testing.xml 两个文件\r\n", Color.Red);
        }

        private void buttontrain_Click(object sender, EventArgs e)
        {
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(train);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.Start(datapath);
        }

        private void buttonmodpath_Click(object sender, EventArgs e)
        {
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

        public void train(object dataDirectory)
        {
            try
            {
                IList<Matrix<RgbPixel>> imagesTrain;
                IList<Matrix<RgbPixel>> imagesTest;
                IList<IList<MModRect>> boxesTrain;
                IList<IList<MModRect>> boxesTest;
                Dlib.LoadImageDataset(dataDirectory + "/training.xml", out imagesTrain, out boxesTrain);
                Dlib.LoadImageDataset(dataDirectory + "/testing.xml", out imagesTest, out boxesTest);

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
                AddInfo($"重叠忽略数量: {numOverlappedIgnored}", 1);
                AddInfo($"额外忽略数量: {numAdditionalIgnored}", 1);
                AddInfo($"重叠测试忽略数量: {numOverlappedIgnoredTest}", 1);


                AddInfo($"训练图像数量: {imagesTrain.Count()}", 1);
                AddInfo($"测试图像数量: {imagesTest.Count()}", 1);

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

                                trainer.SetIterationsWithoutProgressThreshold(50000);
                                trainer.SetTestIterationsWithoutProgressThreshold(1000);

                                const string syncFilename = "mmod_cars_sync";
                                trainer.SetSynchronizationFile(syncFilename, 5 * 60);



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
                                        // Log the training parameters to the console
                                        AddInfo($"{trainer}{cropper}", 1);

                                        var cnt = 1;
                                        // Run the trainer until the learning rate gets small.  
                                        while (trainer.GetLearningRate() >= 1e-4)
                                        {
                                            //30一组
                                            if (cnt % 30 != 0 || !imagesTest.Any())
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
                                            ++cnt;
                                        }
                                        // 等待
                                        trainer.GetNet();
                                        AddInfo("训练完成", 1);

                                        // 保存网络
                                        net.Clean();
                                        LossMmod.Serialize(net, textBoxpath.Text + "/module.dat");
                                        AddInfo($"{trainer}{cropper}", 1);

                                        AddInfo($"同步文件名: {syncFilename}", 1);
                                        AddInfo($"训练图片数量: {imagesTrain.Count()}", 1);
                                        using (var _ = new TestBoxOverlap())
                                        using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTrain, boxesTrain, _, 0, options.OverlapsIgnore))
                                            AddInfo($"训练结果: {matrix}", 1);
                                        Dlib.UpsampleImageDataset(2, imagesTrain, boxesTrain, 1800 * 1800);
                                        using (var _ = new TestBoxOverlap())
                                        using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTrain, boxesTrain, _, 0, options.OverlapsIgnore))
                                            AddInfo($"训练上采样结果: {matrix}", 1);


                                        AddInfo("测试图片数量: {images_test.Count()}", 1);
                                        using (var _ = new TestBoxOverlap())
                                        using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTest, boxesTest, _, 0, options.OverlapsIgnore))
                                            AddInfo($"测试结果: {matrix}", 1);
                                        Dlib.UpsampleImageDataset(2, imagesTest, boxesTest, 1800 * 1800);
                                        using (var _ = new TestBoxOverlap())
                                        using (var matrix = Dlib.TestObjectDetectionFunction(net, imagesTest, boxesTest, _, 0, options.OverlapsIgnore))
                                            AddInfo($"测试上采样结果: {matrix}", 1);

                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                AddInfo(ex.Message, 2);
            }
        }

        private static int IgnoreOverlappedBoxes(IList<MModRect> boxes, TestBoxOverlap overlaps)
        {
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
    }
}

