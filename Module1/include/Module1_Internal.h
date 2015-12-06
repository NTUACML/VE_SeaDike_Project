#pragma once
#include "Module1_Var.h"
#include <vector>
#include <fstream>
class Module1_Internal
{
public:
	//Constructor
	Module1_Internal();

	//Distructor
	~Module1_Internal();

	//Public Func
	void SetVar(Module1_Var *Var);
private:
	Module1_Var *Gv;
};

