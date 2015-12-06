#pragma once
#include <vector>
struct Point
{
	Point():x(0.0),y(0.0){};
	Point(double _x, double _y) :x(_x), y(_y) {};
	double x, y;
};
class Block
{
public:
	//Constructor
	Block():Density(1.0),FrictionC(1.0){};
	Block(double _Den, double _Fri) :Density(_Den), FrictionC(_Fri) {};
	//Distructor
	~Block(){};
	//Public data
	std::vector<Point> Node; // Data coorodinate
	double Density; //Block density
	double FrictionC;
};