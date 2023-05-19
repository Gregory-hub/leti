#include <iostream>
#include <opencv2/opencv.hpp>
#include <vector>
#include <math.h>
#include <numeric>
#include <fstream>
#include <string>
#include <unordered_map>
#include <queue>
#include <bitset>

using namespace std;
using namespace cv;

const int luminance[8][8] =
{ 16, 11, 10, 16, 24, 40, 51, 61,
  12, 12, 14, 19, 26, 58, 60, 55,
  14, 13, 16, 24, 40, 57, 69, 56,
  14, 17, 22, 29, 51, 87, 80, 62,
  18, 22, 37, 56, 68,109,103, 77,
  24, 35, 55, 64, 81,104,113, 92,
  49, 64, 78, 87,103,121,120,101,
  72, 92, 95, 98,112,100,103, 99 };

const int color[8][8] =
{ 17, 18, 24, 47, 99, 99, 99, 99,
  18, 21, 26, 66, 99, 99, 99, 99,
  24, 26, 56, 99, 99, 99, 99, 99,
  47, 66, 99, 99, 99, 99, 99, 99,
  99, 99, 99, 99, 99, 99, 99, 99,
  99, 99, 99, 99, 99, 99, 99, 99,
  99, 99, 99, 99, 99, 99, 99, 99,
  99, 99, 99, 99, 99, 99, 99, 99 };

const int ZigZag[64] = {
	0, 1, 5, 6, 14, 15, 27, 28,
	2, 4, 7, 13, 16, 26, 29, 42,
	3, 8, 12, 17, 25, 30, 41, 43,
	9, 11, 18, 24, 31, 40, 44, 53,
	10, 19, 23, 32, 39, 45, 52, 54,
	20, 22, 33, 38, 46, 51, 55, 60,
	21, 34, 37, 47, 50, 56, 59, 61,
	35, 36, 48, 49, 57, 58, 62, 63
};

const int BYTE_SIZE = 8; // number of bits in a byte

const double PI = 3.14159265358979323846;

void subsample(Mat& cb, Mat& cr, int rows, int cols)
{
	Mat sub_cb(rows / 2, cols / 2, cb.type());
	Mat sub_cr(rows / 2, cols / 2, cr.type());

	for (int i = 0; i < rows; i += 2)
	{
		for (int j = 0; j < cols; j += 2)
		{
			int sum_cb = 0, sum_cr = 0, count_cb = 0, count_cr = 0;

			for (int k = 0; k < 2; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					if (i + k < rows && j + l < cols)
					{
						sum_cb += cb.at<uchar>(i + k, j + l);
						count_cb++;

						sum_cr += cr.at<uchar>(i + k, j + l);
						count_cr++;
					}

				}
			}
			sub_cb.at<uchar>(i / 2, j / 2) = sum_cb / count_cb;
			sub_cr.at<uchar>(i / 2, j / 2) = sum_cr / count_cr;
		}
	}

	cb = sub_cb;
	cr = sub_cr;
}

void re_subsample(Mat& cb, Mat& cr, int rows, int cols)
{
	Mat up_cb(rows * 2, cols * 2, cb.type());
	Mat up_cr(rows * 2, cols * 2, cr.type());

	for (int i = 0; i < rows * 2; i += 2)
	{
		for (int j = 0; j < cols * 2; j += 2)
		{
			up_cb.at<uchar>(i, j) = cb.at<uchar>(i / 2, j / 2);
			up_cr.at<uchar>(i, j) = cr.at<uchar>(i / 2, j / 2);

			if (j + 1 < cols * 2)
			{
				up_cb.at<uchar>(i, j + 1) = cb.at<uchar>(i / 2, j / 2);
				up_cr.at<uchar>(i, j + 1) = cr.at<uchar>(i / 2, j / 2);
			}

			if (i + 1 < rows * 2)
			{
				up_cb.at<uchar>(i + 1, j) = cb.at<uchar>(i / 2, j / 2);
				up_cr.at<uchar>(i + 1, j) = cr.at<uchar>(i / 2, j / 2);

				if (j + 1 < cols * 2)
				{
					up_cb.at<uchar>(i + 1, j + 1) = cb.at<uchar>(i / 2, j / 2);
					up_cr.at<uchar>(i + 1, j + 1) = cr.at<uchar>(i / 2, j / 2);
				}
			}
		}
	}

	cb = up_cb;
	cr = up_cr;
}

void IntoBlocks(Mat& channel, vector<Mat>& blocks)
{
	int blockCount = (channel.rows / 8) * (channel.cols / 8);
	int rows = channel.rows / 8;
	int cols = channel.cols / 8;
	blocks.clear();

	for (int i = 0; i < rows; i++) {
		for (int j = 0; j < cols; j++) {
			Rect rect(j * 8, i * 8, 8, 8);
			blocks.push_back(channel(rect));
		}
	}
	//cout << "\n\nHow many " << blockCount;
}

void dct(Mat& block, Mat& dctBlock)
{
	dctBlock.create(block.size(), block.type());

	for (int x = 0; x < 8; x++)
	{
		for (int y = 0; y < 8; y++)
		{
			block.at<float>(x, y) -= 128;
		}
	}

	const int N = 8;
	double cos1, cos2, sum;

	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			sum = 0;
			for (int y = 0; y < 8; y++) {
				for (int x = 0; x < 8; x++) {
					cos1 = cos(((2 * x + 1) * j * PI) / (2.0 * N));
					cos2 = cos(((2 * y + 1) * i * PI) / (2.0 * N));
					sum += block.at<float>(y, x) * cos1 * cos2;
				}
			}

			if (i == 0) sum /= std::sqrt(2.0);
			if (j == 0) sum /= std::sqrt(2.0);
			dctBlock.at<float>(u, v) = sum / 4.0;
		}
	}
}

void dct_block(Mat& block)
{
	Mat dctBlock;
	dct(block, dctBlock);
	block = dctBlock;
}

void idct(Mat& block, Mat& dctBlock)
{
	dctBlock.create(block.size(), block.type());

	double sum;
	double value;
	double cos1;
	double cos2;
	for (int y = 0; y < 8; y++)
	{
		for (int x = 0; x < 8; x++)
		{
			sum = 0;
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					cos1 = cos((2 * x + 1) * j * PI / 16);
					cos2 = cos((2 * y + 1) * i * PI / 16);
					value = block.at<float>(i, j) * cos1 * cos2;
					if (i == 0) value /= std::sqrt(2);
					if (j == 0) value /= std::sqrt(2);

					sum += value;
				}
			}
			detransformed[y, x] = sum / 4;
		}
	}

	for (int x = 0; x < 8; x++)
	{
		for (int y = 0; y < 8; y++)
		{
			block.at<float>(x, y) += 128;
			if (block.at<float>(x, y) < 0) block.at<float>(x, y) = 0;
			if (block.at<float>(x, y) > 255) block.at<float>(x, y) = 255;
		}
	}
}

void re_dct_block(Mat& block)
{
	Mat redctBlock;
	idct(block, redctBlock);
	block = redctBlock;
}

void quantization_y_block(Mat& block, int quality) {
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			block.at<float>(i, j) = round(block.at<float>(i, j) / (luminance[i][j] * quality));
			//if (block.at<float>(i, j) == -0)
			//	block.at<float>(i, j) = 0;
		}
	}
}

void re_quantization_y_block(Mat& block, int quality) {
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			block.at<float>(i, j) = round(block.at<float>(i, j) * (luminance[i][j] * quality));

		}
	}
}

void quantization_cbcr_block(Mat& block, int quality) {
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			block.at<float>(i, j) = round(block.at<float>(i, j) / (color[i][j] * quality));
			//if (block.at<float>(i, j) == -0)
			//	block.at<float>(i, j) = 0;
		}
	}
}

void re_quantization_cbcr_block(Mat& block, int quality) {
	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			block.at<float>(i, j) = round(block.at<float>(i, j) * (color[i][j] * quality));

		}
	}
}

void zigzag(Mat block, int zig_zag[64])
{
	int index = 0;
	for (int i = 0; i < 8; i++)
	{
		for (int j = 0; j < 8; j++)
		{
			int k = ZigZag[index++];
			zig_zag[k] = block.at<float>(i, j);
		}
	}
}

void reverse_zigzag(vector<int>& compressed_block, int* uncompressed_block) {

	for (int i = 0; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			int index = ZigZag[i * 8 + j];
			uncompressed_block[i * 8 + j] = compressed_block[index];
		}
	}
}

string rle_encode(int* text, int size) {
	string result = "";
	int count = 1;
	int current = text[0];
	for (int i = 1; i < size; i++) {
		if (text[i] == current) {
			count++;
		}
		else {
			result += (char)count;
			result += (char)current;
			current = text[i];
			count = 1;
		}
	}
	result += (char)count;
	result += (char)current;
	return result;
}

vector<int> rle_decode(string encoded) {
	vector<int> result;
	for (int i = 0; i < encoded.length(); i += 2) {
		int count = (int)encoded[i];
		int value = (int)encoded[i + 1];
		for (int j = 0; j < count; j++) {
			result.push_back(value);
		}
	}
	return result;
}

struct Node_huff {
	char val;
	int freq;
	Node_huff* left;
	Node_huff* right;
	Node_huff(char c, int f) : val(c), freq(f), left(nullptr), right(nullptr) {}
};

struct compareNode_huffs {
	bool operator()(Node_huff* a, Node_huff* b) {
		return a->freq > b->freq;
	}
};

Node_huff* buildTree(unordered_map<char, int>& freq) {
	priority_queue<Node_huff*, vector<Node_huff*>, compareNode_huffs> pqueue;
	for (auto pair : freq) {
		pqueue.push(new Node_huff(pair.first, pair.second));
	}

	while (pqueue.size() > 1) {
		Node_huff* left = pqueue.top();
		pqueue.pop();
		Node_huff* right = pqueue.top();
		pqueue.pop();
		Node_huff* parent = new Node_huff('\0', left->freq + right->freq);
		parent->left = left;
		parent->right = right;
		pqueue.push(parent);
	}
	return pqueue.top();
}

void getCodesHelper(Node_huff* root, string code, unordered_map<char, string>& codes) {
	if (root->left == nullptr && root->right == nullptr) {
		codes[root->val] = code;
		return;
	}
	getCodesHelper(root->left, code + "0", codes);
	getCodesHelper(root->right, code + "1", codes);
}

unordered_map<char, string> getCodes(Node_huff* root) {
	unordered_map<char, string> codes;
	getCodesHelper(root, "", codes);
	return codes;
}

string encode(string text, unordered_map<char, string>& codes) {
	string result;
	for (char c : text) {
		result += codes[c];
	}
	return result;
}

string decode(Node_huff* root, string encoded) {
	string result;
	Node_huff* curr = root;
	for (char c : encoded) {
		if (c == '0') {
			curr = curr->left;
		}
		else {
			curr = curr->right;
		}
		if (curr->left == nullptr && curr->right == nullptr) {
			// found a leaf node (i.e., a character)
			result += curr->val;
			curr = root;
		}
	}
	return result;
}

void merge_blocks(vector<Mat>& blocks, Mat& output) {
	int rows_per_block = blocks[0].rows;
	int cols_per_block = blocks[0].cols;
	int rows = output.rows;
	int cols = output.cols;

	for (int i = 0; i < blocks.size(); i++) {
		int row_idx = (i / (cols / cols_per_block)) * rows_per_block;
		int col_idx = (i % (cols / cols_per_block)) * cols_per_block;
		blocks[i].copyTo(output(Rect(col_idx, row_idx, cols_per_block, rows_per_block)));
	}
}

int main()
{
	int quality = 1;

	fstream file_out_rle, file_out_rle_y, file_out_rle_cb, file_out_rle_cr, f_outrle, file_out_huff, file_out_enhuff;
	string all_rle = "";

	file_out_rle.open("out_rle.txt", ios::out);
	file_out_rle_y.open("out_rle_y.txt", ios::out);
	file_out_rle_cb.open("out_rle_cb.txt", ios::out);
	file_out_rle_cr.open("out_rle_cr.txt", ios::out);
	//f_outrle.open("outrle.txt", ios::out);
	file_out_huff.open("out_huff.txt", ios::out);
	file_out_enhuff.open("out_enhuff.txt", ios::out);

	Mat image = imread("jpeg.jpg");

	if (image.empty())
	{
		cout << "Image not found" << endl;
		return 0;
	}

	const int rows = image.rows;
	const int cols = image.cols;

	Mat ycbcrImage;
	cvtColor(image, ycbcrImage, COLOR_BGR2YCrCb);

	vector<Mat> ycbcrChannels(3);
	split(ycbcrImage, ycbcrChannels);
	Mat y = ycbcrChannels[0];
	Mat cb = ycbcrChannels[1];
	Mat cr = ycbcrChannels[2];

	imwrite("Y.jpg", y);
	imwrite("Cb.jpg", cb);
	imwrite("Cr.jpg", cr);

	subsample(cb, cr, rows, cols);

	imwrite("Cb_1.jpg", cb);
	imwrite("Cr_1.jpg", cr);

	//re_subsample(cb, cr, rows / 2, cols / 2);
	//imwrite("Cb_2.jpg", cb);
	//imwrite("Cr_2.jpg", cr);

	//vector<Mat> mergedChannels = { y, cb, cr };
	//Mat mergedImage;
	//merge(mergedChannels, mergedImage);

	//Mat outputImage;
	//cvtColor(mergedImage, outputImage, COLOR_YCrCb2BGR);

	//imwrite("output_1.jpg", outputImage);

	vector<Mat> y_blocks;
	vector<Mat> cb_blocks;
	vector<Mat> cr_blocks;

	IntoBlocks(y, y_blocks);
	IntoBlocks(cb, cb_blocks);
	IntoBlocks(cr, cr_blocks);

	for (int i = 0; i < y_blocks.size(); i++)
	{
		y_blocks[i].convertTo(y_blocks[i], CV_32F);

		Mat& y_block = y_blocks[i];
		/*cout << "Before DCT Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < y_block.rows; i++)
		{
			for (int j = 0; j < y_block.cols; j++)
			{
				cout << y_block.at<float>(i, j) << " ";
			}
			cout << endl;
		}*/

		dct_block(y_blocks[i]);

		/*cout << "After DCT Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < y_block.rows; i++)
		{
			for (int j = 0; j < y_block.cols; j++)
			{
				cout << y_block.at<float>(i, j) << " ";
			}
			cout << endl;
		}*/

		quantization_y_block(y_blocks[i], quality);

		/*cout << "Quant Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < y_block.rows; i++)
		{
			for (int j = 0; j < y_block.cols; j++)
			{
				cout << y_block.at<float>(i, j) << " ";
			}
			cout << endl;
		}*/

		int zig_zag_y[64];
		zigzag(y_blocks[i], zig_zag_y);

		/*cout << "ZigZag Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < 64; i++)
		{
				cout << zig_zag_y[i] << " ";
				//if ((i + 1) % 8 == 0) cout << endl;
		}*/

		string rle_encoded_y = rle_encode(zig_zag_y, 64);

		//cout << "\nRLE encode Coefficients for block " << i << ": " << endl;
		//cout << rle_encoded_y << " ";
		//cout << endl;

		file_out_rle << rle_encoded_y;
		all_rle += rle_encoded_y;
		file_out_rle_y << rle_encoded_y;

		vector<int> rle_decod_y = rle_decode(rle_encoded_y);
		/*cout << "\nRLE decode Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < rle_decod_y.size(); i++) {
			std::cout << rle_decod_y[i] << " ";
		}*/

		int uncompressed_block_y[64];
		reverse_zigzag(rle_decod_y, uncompressed_block_y);

		/*cout << "reZigZag Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < 64; i++)
		{
			cout << uncompressed_block_y[i] << " ";
			//if ((i + 1) % 8 == 0) cout << endl;
		}*/

		Mat mat_y(8,8, CV_32F);
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				mat_y.at<float>(i, j) = uncompressed_block_y[i * 8 + j];
				//cout  << mat_y.at<int>(i, j) << " ";
			}
		}

		re_quantization_y_block(mat_y, quality);

		/*cout << "reQuant Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < mat_y.rows; i++)
		{
			for (int j = 0; j < mat_y.cols; j++)
			{
				cout << mat_y.at<float>(i, j) << " ";
			}
			cout << endl;
		}*/

		re_dct_block(mat_y);
		
		/*cout << "reDCT Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < mat_y.rows; i++)
		{
			for (int j = 0; j < mat_y.cols; j++)
			{
				cout << mat_y.at<float>(i, j) << " ";
			}
			cout << endl;
		}*/

		y_blocks[i] = mat_y;

	}

	for (int i = 0; i < cb_blocks.size(); i++)
	{
		cb_blocks[i].convertTo(cb_blocks[i], CV_32F);

		//Mat& cb_block = cb_blocks[i];
		/*cout << "Before dct for block " << i << ": " << endl;
		for (int i = 0; i < cb_block.rows; i++)
		{
			for (int j = 0; j < cb_block.cols; j++)
			{
				cout << cb_block.at<float>(i, j) << " ";
			}
			cout << endl;
		}*/

		dct_block(cb_blocks[i]);

		/*cout << "DCT Coefficients for block " << i << ": " << endl;
		for (int i = 0; i < cb_block.rows; i++)
		{
			for (int j = 0; j < cb_block.cols; j++)
			{
				cout << cb_block.at<float>(i, j) << " ";
			}
			cout << endl;
		}*/

		quantization_cbcr_block(cb_blocks[i], quality);

		int zig_zag_cb[64];
		zigzag(cb_blocks[i], zig_zag_cb);

		string rle_encoded_cb = rle_encode(zig_zag_cb, 64);

		file_out_rle << rle_encoded_cb;
		all_rle += rle_encoded_cb;
		file_out_rle_cb << rle_encoded_cb;

		vector<int> rle_decod_cb = rle_decode(rle_encoded_cb);

		int uncompressed_block_cb[64];
		reverse_zigzag(rle_decod_cb, uncompressed_block_cb);

		Mat mat_cb(8, 8, CV_32F);
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				mat_cb.at<float>(i, j) = uncompressed_block_cb[i * 8 + j];
			}
		}

		re_quantization_y_block(mat_cb, quality);

		re_dct_block(mat_cb);

		cb_blocks[i] = mat_cb;
	}

	for (int i = 0; i < cr_blocks.size(); i++)
	{
		cr_blocks[i].convertTo(cr_blocks[i], CV_32F);

		dct_block(cr_blocks[i]);

		quantization_cbcr_block(cr_blocks[i], quality);

		int zig_zag_cr[64];
		zigzag(cr_blocks[i], zig_zag_cr);

		string rle_encoded_cr = rle_encode(zig_zag_cr, 64);

		file_out_rle << rle_encoded_cr;
		all_rle += rle_encoded_cr;
		file_out_rle_cr << rle_encoded_cr;

		vector<int> rle_decod_cr = rle_decode(rle_encoded_cr);

		int uncompressed_block_cr[64];
		reverse_zigzag(rle_decod_cr, uncompressed_block_cr);

		Mat mat_cr(8, 8, CV_32F);
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				mat_cr.at<float>(i, j) = uncompressed_block_cr[i * 8 + j];
			}
		}

		re_quantization_y_block(mat_cr, quality);

		re_dct_block(mat_cr);

		cr_blocks[i] = mat_cr;
	}

	unordered_map<char, int> freq;
	for (char c : all_rle) {
		freq[c]++;
	}
	Node_huff* root = buildTree(freq);
	unordered_map<char, string> codes = getCodes(root);
	string encoded = encode(all_rle, codes);
	string decoded = decode(root, encoded);
	for (int i = 0; i < encoded.length(); i += BYTE_SIZE) {
		// Convert every BYTE_SIZE bits (1 byte) of encoded string into a character
		unsigned char byte = bitset<BYTE_SIZE>(encoded.substr(i, BYTE_SIZE)).to_ulong();
		file_out_enhuff << byte;
	}
	file_out_huff << decoded;

	//f_outrle << all_rle;

	file_out_rle.close();
	file_out_rle_y.close();
	file_out_rle_cb.close();
	file_out_rle_cr.close();
	//f_outrle.close();

	merge_blocks(y_blocks, y);
	merge_blocks(cb_blocks, cb);
	merge_blocks(cr_blocks, cr);

	re_subsample(cb, cr, rows / 2, cols / 2);
	imwrite("Cb_2.jpg", cb);
	imwrite("Cr_2.jpg", cr);

	vector<Mat> merged_blocks = { y, cb, cr };
	Mat merged_image;
	merge(merged_blocks, merged_image);
	cvtColor(merged_image, merged_image, COLOR_YCrCb2BGR);

	// Display the final image
	imwrite("Final_Image.jpg", merged_image);

	return 0;
}
