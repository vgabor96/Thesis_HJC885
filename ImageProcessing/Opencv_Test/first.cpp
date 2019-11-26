#include<opencv2/opencv.hpp>
#include<iostream>
using namespace std;
using namespace cv;
int main()
{
	//load the image
	Mat img = imread("lena.jpg");
	namedWindow("oldlena", WINDOW_NORMAL);
	imshow("oldlena", img);

	//read height, width data
	int height = img.rows;
	int width = img.cols;


	//check whether image loaded is empty or not.
	if (img.empty())
	{
		cerr << "no image"; return -1;
	}

	//convert image from CV::MAT to float*.
	Mat dst;
	img.convertTo(dst, CV_32F);
	float* data = dst.ptr<float>();

	//Grayscale
	for (int i = 0; i < (height*width)*3; i+=3)
	{
		float r = data[i];
		float g = data[i + 1];
		float b = data[i + 2];

		float avg = (r + g + b) / 3;
		data[i] = avg;
		data[i + 1] = avg;
		data[i + 2] = avg;
	}
	

	//convert back the image from float* to CV::MAT.
	Mat dest(height, width, CV_32FC3, data);

	//print the image
	imwrite("newlena.jpg", dest);
	Mat img2 = imread("newlena.jpg");
	namedWindow("newlena", WINDOW_NORMAL);
	imshow("newlena", img2);

	waitKey(0);
	return 0;
}