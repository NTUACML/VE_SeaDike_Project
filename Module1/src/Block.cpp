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
	double C_x, C_y;
	C_x = C_y = 0.0;
	for (size_t i = 0; i < Node.size(); i++)
	{
		C_x += Node[i].x;
		C_y += Node[i].y;
	}
	WeightC.x = C_x / double(Node.size());
	WeightC.y = C_y / double(Node.size());
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

