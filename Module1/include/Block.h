#pragma once
#include <vector>;
struct Point
{
	Point():x(0.0),y(0.0){};
	double x, y;
};
class Block
{
public:
	//Constructor
	Block():Density(1.0),FrictionC(1.0){};
	//Distructor
	~Block(){};
	//Public data
	std::vector<Point> Node; // Data coorodinate
	double Density; //Block density
	double FrictionC;
};