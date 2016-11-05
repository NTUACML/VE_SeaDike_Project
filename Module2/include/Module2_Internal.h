#pragma once
#include <iostream>
#include "Module2_Var.h"
class Module2_Internal
{
public:
	Module2_Internal();
	virtual ~Module2_Internal();

	//Public Func
	void SetVar(Module2_Var *_Var);
private:
	Module2_Var *Var;

};

