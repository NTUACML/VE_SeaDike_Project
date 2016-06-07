#pragma once
#include <vector>
struct Point
{
	Point():x(0.0),y(0.0){};
	Point(double _x, double _y) :x(_x), y(_y) {};
	double cross(Point & B) {
		return (x * B.y - B.x * y);
	}
	double x, y;
};
class Block
{
public:
	//Constructor
	Block():Density(1.0),FrictionC(1.0), CalMoment(true){};
	Block(double _Den, double _Fri, bool _CalM) :Density(_Den), FrictionC(_Fri), CalMoment(_CalM) {};
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
	double SelfWeight; //Weight for each block
	double Mw; //Block self Moment
	bool CalMoment;
	//Public Function
	void Cal_Area(); //Calculate Area
	void Cal_WeightC();
	void Cal_MinMax();
};