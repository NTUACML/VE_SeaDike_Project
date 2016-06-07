#pragma once
#include "..\include\Block.h"

void Block::Cal_Area()
{
	Area = 0.0;
	for (size_t i = Node.size() - 1, j = 0; j < Node.size(); i = j++)
	{
		Area += Node[i].x * Node[j].y;
		Area -= Node[i].y * Node[j].x;
	}
	Area *= 0.5;
}

void Block::Cal_WeightC()
{
	double C_x, C_y, w;
	C_x = C_y = w = 0.0;

	for (size_t i = Node.size() - 1, j = 0; j < Node.size(); i = j++)
	{
		double a = Node[i].cross(Node[j]);
		C_x += (Node[i].x + Node[j].x) * a;
		C_y += (Node[i].y + Node[j].y) * a;
		w += a;
	}

	WeightC.x = C_x / 3.0 / w;
	WeightC.y = C_y / 3.0 / w;
}

void Block::Cal_MinMax()
{
	MinLevel = WeightC.y;
	MaxLevel = WeightC.y;
	MaxX = WeightC.x;
	MinX = WeightC.x;
	for (size_t i = 0; i < Node.size(); i++)
	{
		if (Node[i].y <= MinLevel) {
			MinLevel = Node[i].y;
		}
		if (Node[i].y >= MaxLevel) {
			MaxLevel = Node[i].y;
		}
		if (Node[i].x <= MinX) {
			MinX = Node[i].x;
		}
		if (Node[i].x >= MaxX) {
			MaxX = Node[i].x;
		}
	}
}

