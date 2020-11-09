using CNTK;
using RobotNeuralNetwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNeuralNetwork
{
    class NeuralNetwork
    {

        const int batchSize = 1000;
        const int epochCount = 1000;

        readonly Variable x;
        readonly Function y;
        readonly Parameter w1, b, w2;

        /*    public BasicNeuralNetwork()
            {
                //Build Graph
                x = Variable.InputVariable(new int[] { inputSize }, DataType.Float);

                w1 = new Parameter(new int[] { hiddenNeuronCount, inputSize },
                    DataType.Float, CNTKLib.GlorotNormalInitializer());

                b = new Parameter(new int[] { hiddenNeuronCount },
                       DataType.Float, CNTKLib.GlorotNormalInitializer());

                w2 = new Parameter(new int[] { outputSize, hiddenNeuronCount },
                    DataType.Float, CNTKLib.GlorotNormalInitializer());

                y = CNTKLib.Sigmoid(CNTKLib.Times(w2, CNTKLib.Sigmoid(CNTKLib.Plus(CNTKLib.Times(w1, x), b))));

            }
            */
        public NeuralNetwork(int hiddenNeuronCount)
        {
            int[] layers = new int[] {DataSet.InputSize, hiddenNeuronCount, hiddenNeuronCount,DataSet.OutputSize};


            //Build graph
            x = Variable.InputVariable(new int[] { layers[0] }, DataType.Float);

            Function lastLayer = x;

            for (int i = 0; i < layers.Length - 1; i++)
            {
                Parameter weight = new Parameter(new int[] { layers[i + 1], layers[i] }, DataType.Float, CNTKLib.GlorotNormalInitializer());
                Parameter bias = new Parameter(new int[] { layers[i + 1] }, DataType.Float, CNTKLib.GlorotNormalInitializer());



                Function times = CNTKLib.Times(weight, lastLayer);
                Function plus = CNTKLib.Plus(times, bias);

                if (i != layers.Length - 2)
                {
                    lastLayer = CNTKLib.Sigmoid(plus);
                }
                else
                {
                    lastLayer = CNTKLib.Softmax(plus);
                }


            }

            y = lastLayer;

        }

        public void Train(string[] trainData)
        {
            //y= SIG(w2*SIG(w1*x+b))

            int n = trainData.Length;

            //Extend graph          
            Variable yt = Variable.InputVariable(new int[] { DataSet.OutputSize }, DataType.Float);

            //Function sqDiff = CNTKLib.Square(CNTKLib.Minus(y, yt));
            //Function loss = CNTKLib.ReduceSum(sqDiff, Axis.AllAxes());
            //Function err = CNTKLib.ClassificationError(y, yt);
            Function loss = CNTKLib.CrossEntropyWithSoftmax(y, yt);
            Function err = CNTKLib.ClassificationError(y, yt);

            Learner learner = CNTKLib.SGDLearner(new ParameterVector(y.Parameters().ToArray()), new TrainingParameterScheduleDouble(1.0, batchSize));
            Trainer trainer = Trainer.CreateTrainer(y, loss, err, new List<Learner>() { learner });


            //TRAIN
            for (int epochi = 0; epochi <= epochCount; epochi++)
            {
                double sumLoss = 0;
                // double sumEval = 0;
                foreach (string line in trainData)
                {
                    float[] values = line.Split(',').Select(x => float.Parse(x)).ToArray();

                    var inputDataMap = new Dictionary<Variable, Value>()
                    {
                        { x, LoadInput(values[0],values[1], values[2], values[3]) },
                        { yt ,Value.CreateBatch(yt.Shape,new float[] { values[4] }, DeviceDescriptor.CPUDevice) }
                    };

                    var outputDataMap = new Dictionary<Variable, Value>() { { loss, null } };

                    trainer.TrainMinibatch(inputDataMap, false, DeviceDescriptor.CPUDevice);
                    loss.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);
                    //sumLoss += trainer.PreviousMinibatchLossAverage();
                    //sumEva += trainer.PreviousMinibatchEvaluationAverage();
                    sumLoss += outputDataMap[loss].GetDenseData<float>(loss)[0][0];
                }


                // float w1Value = new Value(w.GetValue()).GetDenseData<float>(w)[0][0];
                //float w2Value = new Value(w.GetValue()).GetDenseData<float>(w)[0][1];
                Console.WriteLine(string.Format("{0}\tloss:{1}", epochi, sumLoss / n));
            }


        }


        public float Prediction(float center1x, float center1y, float center2x, float center2y)
        {
            //"F1 = 2* (pontosság * szenz/(pontosság + szenz))"
            var inputDataMap = new Dictionary<Variable, Value>() { { x, LoadInput(center1x, center1y, center2x, center2y) } };
            var outputDataMap = new Dictionary<Variable, Value>() { { y, null } };

            y.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);


            return outputDataMap[y].GetDenseData<float>(y)[0][0];
        }
        Value LoadInput(float center1x, float center1y, float center2x, float center2y)
        {
            float[] x_store = new float[DataSet.InputSize];
            x_store[0] = center1x;
            x_store[1] = center1y;
            x_store[2] = center2x;
            x_store[3] = center2y;

            return Value.CreateBatch(x.Shape, x_store, DeviceDescriptor.CPUDevice);
        }


    }

    //class BasicNeuralNetwork
    //{
    //    const int inputSize = 4;
    //    const int hiddenNeuronCount = 3;
    //    const int outputSize = 1;

    //    readonly Variable x;
    //    readonly Function y;
    //    readonly Parameter w1, b, w2;

    //    public BasicNeuralNetwork()
    //    {
    //        //Build Graph
    //        x = Variable.InputVariable(new int[] { inputSize }, DataType.Float);

    //        w1 = new Parameter(new int[] { hiddenNeuronCount, inputSize },
    //            DataType.Float, CNTKLib.GlorotNormalInitializer());

    //        b = new Parameter(new int[] { hiddenNeuronCount },
    //               DataType.Float, CNTKLib.GlorotNormalInitializer());

    //        w2 = new Parameter(new int[] { outputSize, hiddenNeuronCount },
    //            DataType.Float, CNTKLib.GlorotNormalInitializer());

    //        y = CNTKLib.Sigmoid(CNTKLib.Times(w2, CNTKLib.Sigmoid(CNTKLib.Plus(CNTKLib.Times(w1, x), b))));

    //    }

    //    public void Train(string[] trainData)
    //    {
    //        //y= SIG(w2*SIG(w1*x+b))

    //        int n = trainData.Length;

    //        //Extend graph          
    //        Variable yt = Variable.InputVariable(new int[] { 1, outputSize }, DataType.Float);

    //        Function sqDiff = CNTKLib.Square(CNTKLib.Minus(y, yt));
    //        Function loss = CNTKLib.ReduceSum(sqDiff, Axis.AllAxes());
    //        // Function loss = CNTKLib.BinaryCrossEntropy(y, yt);

    //        //Function y_rounded = CNTKLib.Round(y);
    //        //Function y_yt_equal = CNTKLib.Equal(y_rounded, yt);

    //        Learner learner = CNTKLib.SGDLearner(new ParameterVector() { w1, b, w2 }, new TrainingParameterScheduleDouble(0.01, 1));
    //        //Learner learner = CNTKLib.SGDLearner(new ParameterVector(y.Parameters().ToArray()) { }, new TrainingParameterScheduleDouble(0.01, 1));

    //        Trainer trainer = Trainer.CreateTrainer(loss, loss, null, new List<Learner>() { learner });
    //        //Trainer trainer = Trainer.CreateTrainer(y, loss, y_yt_equal, new List<Learner>() { learner });

    //        //TRAIN
    //        for (int i = 0; i <= 100; i++)
    //        {
    //            double sumLoss = 0;
    //            // double sumEval = 0;
    //            foreach (string line in trainData)
    //            {
    //                float[] values = line.Split('\t').Select(x => float.Parse(x)).ToArray();

    //                var inputDataMap = new Dictionary<Variable, Value>()
    //                {
    //                    { x, LoadInput(values[0],values[1], values[2], values[3]) },
    //                    { yt ,Value.CreateBatch(yt.Shape,new float[] { values[4] }, DeviceDescriptor.CPUDevice) }
    //                };

    //                var outputDataMap = new Dictionary<Variable, Value>() { { loss, null } };

    //                trainer.TrainMinibatch(inputDataMap, false, DeviceDescriptor.CPUDevice);
    //                loss.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);
    //                //sumLoss += trainer.PreviousMinibatchLossAverage();
    //                //sumEva += trainer.PreviousMinibatchEvaluationAverage();
    //                sumLoss += outputDataMap[loss].GetDenseData<float>(loss)[0][0];
    //            }


    //            // float w1Value = new Value(w.GetValue()).GetDenseData<float>(w)[0][0];
    //            //float w2Value = new Value(w.GetValue()).GetDenseData<float>(w)[0][1];
    //            Console.WriteLine(string.Format("{0}\tloss:{1}", i, sumLoss / n));
    //        }


    //    }
    //    public float Prediction(float age, float height, float weight, float salary)
    //    {
    //        //"F1 = 2* (pontosság * szenz/(pontosság + szenz))"
    //        var inputDataMap = new Dictionary<Variable, Value>() { { x, LoadInput(age, height, weight, salary) } };
    //        var outputDataMap = new Dictionary<Variable, Value>() { { y, null } };

    //        y.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);


    //        return outputDataMap[y].GetDenseData<float>(y)[0][0];
    //    }
    //    Value LoadInput(float age, float height, float weight, float salary)
    //    {
    //        float[] x_store = new float[inputSize];
    //        x_store[0] = age / 100;
    //        x_store[1] = height / 250;
    //        x_store[2] = weight / 150;
    //        x_store[3] = salary / 15000000;

    //        return Value.CreateBatch(x.Shape, x_store, DeviceDescriptor.CPUDevice);
    //    }


    //}

    class BasicNeuralNetwork
    {
        const int inputSize = 4;
        const int hiddenNeuronCount = 100;
        const int outputSize = 1;

        readonly Variable x;
        readonly Function y;
        readonly Parameter w1, b, w2;

        public BasicNeuralNetwork()
        {
            //Build Graph
            x = Variable.InputVariable(new int[] { inputSize }, DataType.Float);

            w1 = new Parameter(new int[] { hiddenNeuronCount, inputSize },
                DataType.Float, CNTKLib.GlorotNormalInitializer());

            b = new Parameter(new int[] { hiddenNeuronCount },
                   DataType.Float, CNTKLib.GlorotNormalInitializer());

            w2 = new Parameter(new int[] { outputSize, hiddenNeuronCount },
                DataType.Float, CNTKLib.GlorotNormalInitializer());

            y = CNTKLib.Sigmoid(CNTKLib.Times(w2, CNTKLib.Sigmoid(CNTKLib.Plus(CNTKLib.Times(w1, x), b))));

        }

        public void Train(string[] trainData)
        {
            //y= SIG(w2*SIG(w1*x+b))

            int n = trainData.Length;

            //Extend graph          
            Variable yt = Variable.InputVariable(new int[] { 1, outputSize }, DataType.Float);

            Function sqDiff = CNTKLib.Square(CNTKLib.Minus(y, yt));
            Function loss = CNTKLib.ReduceSum(sqDiff, Axis.AllAxes());
            Learner learner = CNTKLib.SGDLearner(new ParameterVector() { w1, b, w2 }, new TrainingParameterScheduleDouble(0.001, 1));        
            Trainer trainer = Trainer.CreateTrainer(loss, loss, null, new List<Learner>() { learner });

            //TRAIN
            for (int i = 0; i <= 1000; i++)
            {
                double sumLoss = 0;
                // double sumEval = 0;
                foreach (string line in trainData)
                {
                    float[] values = line.Split(',').Select(x => float.Parse(x)).ToArray();

                    var inputDataMap = new Dictionary<Variable, Value>()
                    {
                        { x, LoadInput(values[0],values[1], values[2], values[3]) },
                        { yt ,Value.CreateBatch(yt.Shape,new float[] { values[4] }, DeviceDescriptor.CPUDevice) }
                    };

                    var outputDataMap = new Dictionary<Variable, Value>() { { loss, null } };

                    trainer.TrainMinibatch(inputDataMap, false, DeviceDescriptor.CPUDevice);
                    loss.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);
                    sumLoss += outputDataMap[loss].GetDenseData<float>(loss)[0][0];
                }
                Console.WriteLine(string.Format("{0}\tloss:{1}", i, sumLoss / n));
            }


        }
        public float Prediction(float center1x, float center1y, float center2x, float center2y)
        {
            //"F1 = 2* (pontosság * szenz/(pontosság + szenz))"
            var inputDataMap = new Dictionary<Variable, Value>() { { x, LoadInput(center1x, center1y, center2x, center2y) } };
            var outputDataMap = new Dictionary<Variable, Value>() { { y, null } };

            y.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);


            return outputDataMap[y].GetDenseData<float>(y)[0][0];
        }
        Value LoadInput(float center1x, float center1y, float center2x, float center2y)
        {
            float[] x_store = new float[inputSize];
            x_store[0] = center1x;
            x_store[1] = center1y;
            x_store[2] = center2x;
            x_store[3] = center2y;

            return Value.CreateBatch(x.Shape, x_store, DeviceDescriptor.CPUDevice);
        }


    }
}

public class Program
{

    string[] trainData = File.ReadAllLines(@"C:\Users\loahc\Documents\GitHub\Thesis_HJC885\ImageProcessing\CUDA_PROJECT\results.txt");
    string[] testdata = File.ReadAllLines(@"C:\Users\loahc\Documents\GitHub\Thesis_HJC885\ImageProcessing\CUDA_PROJECT\tests.txt");

    BasicNeuralNetwork app = new BasicNeuralNetwork();

    void Run()
    {
        app.Train(trainData);
        FileTest(testdata);
        Console.ReadKey();

    }

    void FileTest(string[] testdata)
    {

        double TP = 0, TN = 0, FP = 0, FN = 0;
        int goodPrediction = 0, wrongPrediction = 0;
        double Accuracy = 0;
        double precision = 0;
        double sensitivity = 0;
        double f1_score = 0;
        foreach (string line in testdata)
        {
            float[] values = line.Split(',').Select(x => float.Parse(x)).ToArray();
            float good = values[4];
            float pred = app.Prediction(values[0], values[1], values[2], values[3]);

            if (Math.Round(pred) != values[4])
            {
                Console.WriteLine("---" + pred + "\t" + line);
                wrongPrediction++;

            }
            else
            {
                Console.WriteLine("+++" + pred + "\t" + line);
                goodPrediction++;

            }

            if (Math.Round(pred) == good)
            {
                if (pred >= 0.5)
                {
                    TP++;
                }
                else
                {
                    TN++;
                }
            }
            else
            {
                if (pred < good)
                {
                    FP++;
                }
                else
                {
                    FN++;
                }
            }


        }

        Console.WriteLine(String.Format("Good Prediction:{0} ({1}%", goodPrediction, 100f * goodPrediction / testdata.Count()));

        Console.WriteLine(String.Format("Wrong Prediction:{0} ({1}%", wrongPrediction, 100f * wrongPrediction / testdata.Count()));

        Accuracy = (TP + TN) / (TP + TN + FN + FP);
        precision = TP / (TP + FP);
        sensitivity = TP / (TP + FN);
        f1_score = 2 * ((precision * sensitivity) / (precision + sensitivity));
        Console.WriteLine($"True positive: {TP}\nTrue negative: {TN}\nFalse positive: {FP}\nFalse negative: {FN}\nAccuracy: {Accuracy}\nPrecision: {precision}\nSensitivity: {sensitivity}\nF1 score: {f1_score}");

    }


    void ConsoleTest()
    {
        while (true)
        {
            Console.WriteLine("Center1 X: ");
            float center1x = float.Parse(Console.ReadLine());
            Console.WriteLine("Center1 Y: ");
            float center1y = float.Parse(Console.ReadLine());
            Console.WriteLine("Center2 X: ");
            float center2x = float.Parse(Console.ReadLine());
            Console.WriteLine("Center2 Y: ");
            float center2y = float.Parse(Console.ReadLine());

            Console.WriteLine("Prediction: " + app.Prediction(center1x, center1y, center2x, center2y));

        }





    }
    static void Main()
    {
        new Program().Run();

    }

}
