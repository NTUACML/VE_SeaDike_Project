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
	Point WeightC;
	double Density; //Block density
	double FrictionC; //Friction Coefficient
	double Area; //Block Area
	bool OnSeaSide; //On Sea Side?
	double MinLevel, MaxLevel, MaxX, MinX; //Min Y, Max X, Min X
	//Public Function
	void Cal_Area(); //Calculate Area
	void Cal_WeightC();
	void Cal_MinMax();
};