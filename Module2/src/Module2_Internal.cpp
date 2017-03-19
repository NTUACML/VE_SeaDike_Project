#include "../include/Module2_Internal.h"


Module2_Internal::Module2_Internal():Var(NULL)
{
}


Module2_Internal::~Module2_Internal()
{
}

void Module2_Internal::SetVar(Module2_Var * _Var)
{
	Var = _Var;
}

bool Module2_Internal::GeoPreCal()
{
	Var->Ref_x = 0.0;
	Var->Ref_y = 0.0;
	//Block Preprocess
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->BlockData[i].Cal_Area();
		Var->BlockData[i].Cal_WeightC();
		Var->BlockData[i].Cal_MinMax();
		Var->Ref_x += Var->BlockData[i].WeightC.x;
		Var->Ref_y += Var->BlockData[i].WeightC.y;
	}
	Var->Ref_x /= double(Var->BlockData.size());
	Var->Ref_y /= double(Var->BlockData.size());

	//Get Max and Min level of All Block
	Var->Max_level = Var->Ref_y;
	Var->Min_level = Var->Ref_y;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].MinLevel <= Var->Min_level) Var->Min_level = Var->BlockData[i].MinLevel;
		if (Var->BlockData[i].MaxLevel >= Var->Max_level) Var->Max_level = Var->BlockData[i].MaxLevel;
	}

	//Get EL Level Up block ID and Arm Y

	double maximum_X = 0.0;
	std::vector<size_t> tmp_blockid;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{

		for (size_t j = 0; j < Var->BlockData.size(); j++)
		{
			if (Var->BlockData[j].CalMoment == true && 
				Var->BlockData[j].MinLevel >= Var->LevelSection[i].Level &&
				Var->BlockData[j].MaxX >= maximum_X)
			{
				maximum_X = Var->BlockData[j].MaxX;
			}
		}

		for (size_t j = 0; j < Var->BlockData.size(); j++)
		{
			if (Var->BlockData[j].MinLevel >= Var->LevelSection[i].Level &&
				Var->BlockData[j].MaxX <= maximum_X &&
				std::find(tmp_blockid.begin(), tmp_blockid.end(), j) == tmp_blockid.end())
			{
				Var->LevelSection[i].BlockId.push_back(j);
				tmp_blockid.push_back(j);
			}
		}
	}

	//Base Block Length
	double BaseMin_x, BaseMax_x, Min_weight_y;
	BaseMin_x = Var->Ref_x;
	BaseMax_x = Var->Ref_x;
	Min_weight_y = Var->Ref_y;

	//- Find Min Weight Y
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].WeightC.y <= Min_weight_y) Min_weight_y = Var->BlockData[i].WeightC.y;
	}

	//- Find Node max_x and min_x lower than Weight Y
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].MinLevel <= Min_weight_y) {
			for (size_t j = 0; j < Var->BlockData[i].Node.size(); j++) {
				if (Var->BlockData[i].Node[j].x <= BaseMin_x) BaseMin_x = Var->BlockData[i].Node[j].x;
				if (Var->BlockData[i].Node[j].x >= BaseMax_x) BaseMax_x = Var->BlockData[i].Node[j].x;
			}
		}
	}

	Var->B = std::abs(BaseMax_x - BaseMin_x);

	Var->Err_Msg += "計算幾何前處理完畢! \r\n";
	return true;
}

bool Module2_Internal::WeightCal()
{
	size_t ID;
	double temp_sum_Mx = 0;
	double temp_sum_W = 0;

	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		ID = Var->LevelSection[i].BlockId[0];
		double Ref_x = Var->BlockData[ID].MinX;

		for (size_t j = 1; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];
			if (Var->BlockData[ID].MinX<Ref_x) Ref_x = Var->BlockData[ID].MinX;
		}

		//- 更新下一個elevation的力臂與抵抗彎矩(但仍需考慮新設一變數已儲存兩個不同elevation之總和)
		if (i >= 1) {
			Var->LevelSection[i].pre_sum_W = Var->LevelSection[i - 1].Level_sum_W;
			Var->LevelSection[i].pre_total_arm = Var->LevelSection[i - 1].Level_total_arm-Ref_x;
			Var->LevelSection[i].pre_sum_Mx = Var->LevelSection[i].pre_sum_W*Var->LevelSection[i].pre_total_arm;
			temp_sum_Mx = Var->LevelSection[i].pre_sum_Mx;
			temp_sum_W = Var->LevelSection[i].pre_sum_W;
		}


		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];
			Var->BlockData[ID].SelfWeight = Var->BlockData[ID].Area * Var->BlockData[ID].Density;
			Var->BlockData[ID].X = std::abs(Var->BlockData[ID].WeightC.x - Ref_x);
			Var->BlockData[ID].Mw = Var->BlockData[ID].SelfWeight * Var->BlockData[ID].X;

			//- Temperary summation ofevery level
			temp_sum_W += Var->BlockData[ID].SelfWeight;
			temp_sum_Mx += Var->BlockData[ID].Mw;
		}

		Var->LevelSection[i].Level_sum_W = temp_sum_W;
		Var->LevelSection[i].Level_sum_Mx = temp_sum_Mx;

		Var->LevelSection[i].Level_total_arm = Var->LevelSection[i].Level_sum_Mx / Var->LevelSection[i].Level_sum_W;

	}

	Var->W = temp_sum_W;
	Var->Mw = temp_sum_Mx;

	Var->Err_Msg += "塊體自重力計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::EarthQuakeForceCal()
{
	size_t ID;
	double temp_sum_Me = 0;
	double temp_sum_Fe = 0;

	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		ID = Var->LevelSection[i].BlockId[0];
		double old_Ref_y;
		double Ref_y = Var->BlockData[ID].MinLevel;

		// need to be sure about j starting number-------------
		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];
			if (Var->BlockData[ID].MinLevel<Ref_y) Ref_y = Var->BlockData[ID].MinLevel;
		}

		//- 更新下一個elevation的力臂與抵抗彎矩(但仍需考慮新設一變數已儲存兩個不同elevation之總和)
		if (i >= 1) {
			Var->LevelSection[i].pre_sum_WE = Var->LevelSection[i - 1].Level_sum_WE;
			double Arm_len = abs(old_Ref_y - Ref_y);
			Var->LevelSection[i].pre_total_armE = Var->LevelSection[i - 1].Level_total_armE + Arm_len;
			Var->LevelSection[i].pre_sum_MxE = Var->LevelSection[i].pre_sum_WE*Var->LevelSection[i].pre_total_armE;
			temp_sum_Me = Var->LevelSection[i].pre_sum_MxE;
			temp_sum_Fe = Var->LevelSection[i].pre_sum_WE;
		}

		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];

			Var->BlockData[ID].SelfWeight_E = Var->BlockData[ID].Area * Var->BlockData[ID].EQ_Density* Var->K;
			Var->BlockData[ID].X_E = std::abs(Var->BlockData[ID].WeightC.y - Ref_y);
			Var->BlockData[ID].Mw_E = Var->BlockData[ID].SelfWeight_E * Var->BlockData[ID].X_E;

			//- Temperary summation ofevery level
			temp_sum_Fe += Var->BlockData[ID].SelfWeight_E;
			temp_sum_Me += Var->BlockData[ID].Mw_E;
		}

		Var->LevelSection[i].Level_sum_WE = temp_sum_Fe;
		Var->LevelSection[i].Level_sum_MxE = temp_sum_Me;

		Var->LevelSection[i].Level_total_armE = Var->LevelSection[i].Level_sum_MxE / Var->LevelSection[i].Level_sum_WE;

		old_Ref_y = Ref_y;
	}
	
	//- Sum Total Weight and Moment
	Var->Fe = temp_sum_Me;
	Var->Me = temp_sum_Fe;

	Var->Err_Msg += "塊體地震力計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::HorizontalSoilForceCal() {

	double pi_up, pi_down, length, temp_u, temp_d, lower_fhi;
	//setting up the max level
	double upper_level = Var->Max_level;
	double rh = 0;
	pi_down = (Var->Q + rh)*Var->ka*cos(Var->WallPhi*M_PI / 180);
	double temp_sum_Fh = 0;
	double temp_sum_FhMh = 0;

	//- USUAL SITUATION
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{

		if (i >= 1) {
			Var->LevelSection[i].pre_sum_Fh = Var->LevelSection[i - 1].Level_sum_Fh;
			double Arm_len = upper_level - Var->LevelSection[i].Level;
			Var->LevelSection[i].pre_total_Fhy = Var->LevelSection[i - 1].Level_total_Fhy + Arm_len;
			Var->LevelSection[i].pre_sum_FhMh = Var->LevelSection[i].pre_sum_Fh*Var->LevelSection[i].pre_total_Fhy;
			temp_sum_Fh = Var->LevelSection[i].pre_sum_Fh;
			temp_sum_FhMh = Var->LevelSection[i].pre_sum_FhMh;
		}

		if (Var->LevelSection[i].Level > Var->RWL) {
			pi_up = pi_down;
			length = upper_level - Var->LevelSection[i].Level;
			rh += length*Var->soilR_Earth;
			pi_down = (Var->Q + rh)*Var->ka*cos(Var->WallPhi*M_PI / 180);
			Var->LevelSection[i].Fh = 0.5*(pi_up + pi_down)*length;

			//refresh the upper level
			upper_level = Var->LevelSection[i].Level;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y = temp_u / temp_d;
			//finding the Mh
			Var->LevelSection[i].Fh_Mh = Var->LevelSection[i].Fh*Var->LevelSection[i].Fh_y;

			//summation
			temp_sum_Fh += Var->LevelSection[i].Fh;
			temp_sum_FhMh += Var->LevelSection[i].Fh_Mh;
		}
		else if (upper_level > Var->RWL && Var->RWL > Var->LevelSection[i].Level) {
			//upper layer
			pi_up = pi_down;
			length = upper_level - Var->RWL;
			rh += length*Var->soilR_Earth;
			pi_down = (Var->Q + rh)*Var->ka*cos(Var->WallPhi*M_PI / 180);
			Var->LevelSection[i].Fh = 0.5*(pi_up + pi_down)*length;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y = (temp_u / temp_d)+(Var->RWL-Var->LevelSection[i].Level);
			//finding the Mh
			Var->LevelSection[i].Fh_Mh = Var->LevelSection[i].Fh*Var->LevelSection[i].Fh_y;
			//------------------------------------------------------------------------------
			//lower layer
			pi_up = pi_down;
			length = Var->RWL - Var->LevelSection[i].Level;
			rh += length*Var->soilR_Water;
			pi_down = (Var->Q + rh)*Var->ka*cos(Var->WallPhi*M_PI / 180);
			lower_fhi = 0.5*(pi_up + pi_down)*length;;
			Var->LevelSection[i].Fh += lower_fhi;

			//refresh the upper level
			upper_level = Var->LevelSection[i].Level;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y = temp_u / temp_d;
			//finding the Mh
			Var->LevelSection[i].Fh_Mh += lower_fhi*Var->LevelSection[i].Fh_y;
			Var->LevelSection[i].Fh_y = Var->LevelSection[i].Fh_Mh / Var->LevelSection[i].Fh;

			//summation
			temp_sum_Fh += Var->LevelSection[i].Fh;
			temp_sum_FhMh += Var->LevelSection[i].Fh_Mh;
		}
		else {
			pi_up = pi_down;
			length = upper_level - Var->LevelSection[i].Level;
			rh += length*Var->soilR_Water;
			pi_down = (Var->Q + rh)*Var->ka*cos(Var->WallPhi*M_PI / 180);
			Var->LevelSection[i].Fh = 0.5*(pi_up + pi_down)*length;

			//refresh the upper level
			upper_level = Var->LevelSection[i].Level;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y = temp_u / temp_d;
			//finding the Mh
			Var->LevelSection[i].Fh_Mh = Var->LevelSection[i].Fh*Var->LevelSection[i].Fh_y;

			//summation
			temp_sum_Fh += Var->LevelSection[i].Fh;
			temp_sum_FhMh += Var->LevelSection[i].Fh_Mh;
		}
		Var->LevelSection[i].Level_sum_Fh = temp_sum_Fh;
		Var->LevelSection[i].Level_sum_FhMh = temp_sum_FhMh;

		Var->LevelSection[i].Level_total_Fhy = Var->LevelSection[i].Level_sum_FhMh / Var->LevelSection[i].Level_sum_Fh;

	}

	upper_level = Var->Max_level;
	double rh_E = 0;
	pi_down = (Var->Qe + rh_E)*Var->ka_17*cos(Var->WallPhi*M_PI / 180);
	double temp_sum_Fh_E = 0;
	double temp_sum_FhMh_E = 0;
	//- Earthquake situation
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{

		if (i >= 1) {
			Var->LevelSection[i].pre_sum_Fh_E = Var->LevelSection[i - 1].Level_sum_Fh_E;
			double Arm_len = upper_level - Var->LevelSection[i].Level;
			Var->LevelSection[i].pre_total_Fhy_E = Var->LevelSection[i - 1].Level_total_Fhy_E + Arm_len;
			Var->LevelSection[i].pre_sum_FhMh_E = Var->LevelSection[i].pre_sum_Fh_E*Var->LevelSection[i].pre_total_Fhy_E;
			temp_sum_Fh_E = Var->LevelSection[i].pre_sum_Fh_E;
			temp_sum_FhMh_E = Var->LevelSection[i].pre_sum_FhMh_E;
		}

		if (Var->LevelSection[i].Level > Var->RWL) {
			pi_up = pi_down;
			length = upper_level - Var->LevelSection[i].Level;
			rh_E += length*Var->soilR_Earth;
			pi_down = (Var->Qe + rh_E)*Var->ka_17*cos(Var->WallPhi*M_PI / 180);
			Var->LevelSection[i].Fh_E = 0.5*(pi_up + pi_down)*length;

			//refresh the upper level
			upper_level = Var->LevelSection[i].Level;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y_E = temp_u / temp_d;
			//finding the Mh
			Var->LevelSection[i].Fh_Mh_E = Var->LevelSection[i].Fh_E*Var->LevelSection[i].Fh_y_E;

			//summation
			temp_sum_Fh_E += Var->LevelSection[i].Fh_E;
			temp_sum_FhMh_E += Var->LevelSection[i].Fh_Mh_E;
		}
		else if (upper_level > Var->RWL && Var->RWL > Var->LevelSection[i].Level) {
			//upper layer
			pi_up = pi_down;
			length = upper_level - Var->RWL;
			rh_E += length*Var->soilR_Earth;
			pi_down = (Var->Qe + rh_E)*Var->ka_17*cos(Var->WallPhi*M_PI / 180);
			Var->LevelSection[i].Fh_E = 0.5*(pi_up + pi_down)*length;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y_E = (temp_u / temp_d) + (Var->RWL - Var->LevelSection[i].Level);
			//finding the Mh
			Var->LevelSection[i].Fh_Mh_E = Var->LevelSection[i].Fh_E*Var->LevelSection[i].Fh_y_E;
			//------------------------------------------------------------------------------
			//lower layer
			pi_up = (Var->Qe + rh_E)*Var->ka_33*cos(Var->WallPhi*M_PI / 180);
			length = Var->RWL - Var->LevelSection[i].Level;
			rh_E += length*Var->soilR_Water;
			pi_down = (Var->Qe + rh_E)*Var->ka_33*cos(Var->WallPhi*M_PI / 180);
			lower_fhi = 0.5*(pi_up + pi_down)*length;;
			Var->LevelSection[i].Fh_E += lower_fhi;

			//refresh the upper level
			upper_level = Var->LevelSection[i].Level;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y_E = temp_u / temp_d;
			//finding the Mh
			Var->LevelSection[i].Fh_Mh_E += lower_fhi*Var->LevelSection[i].Fh_y_E;
			Var->LevelSection[i].Fh_y_E = Var->LevelSection[i].Fh_Mh_E / Var->LevelSection[i].Fh_E;

			//summation
			temp_sum_Fh_E += Var->LevelSection[i].Fh_E;
			temp_sum_FhMh_E += Var->LevelSection[i].Fh_Mh_E;
		}
		else {
			pi_up = pi_down;
			length = upper_level - Var->LevelSection[i].Level;
			rh_E += length*Var->soilR_Water;
			pi_down = (Var->Qe + rh_E)*Var->ka_33*cos(Var->WallPhi*M_PI / 180);
			Var->LevelSection[i].Fh_E = 0.5*(pi_up + pi_down)*length;

			//refresh the upper level
			upper_level = Var->LevelSection[i].Level;

			//finding the arm
			temp_u = (pi_up*length)*length*0.5 + (pi_down - pi_up)*length*0.5*length / 3;
			temp_d = pi_up*length + (pi_down - pi_up)*length*0.5;
			Var->LevelSection[i].Fh_y_E = temp_u / temp_d;
			//finding the Mh
			Var->LevelSection[i].Fh_Mh_E = Var->LevelSection[i].Fh_E*Var->LevelSection[i].Fh_y_E;

			//summation
			temp_sum_Fh_E += Var->LevelSection[i].Fh_E;
			temp_sum_FhMh_E += Var->LevelSection[i].Fh_Mh_E;
		}

		Var->LevelSection[i].Level_sum_Fh_E = temp_sum_Fh_E;
		Var->LevelSection[i].Level_sum_FhMh_E = temp_sum_FhMh_E;

		Var->LevelSection[i].Level_total_Fhy_E = Var->LevelSection[i].Level_sum_FhMh_E / Var->LevelSection[i].Level_sum_Fh_E;

	}

	Var->Err_Msg += "土壓水平力及傾倒彎矩計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::VertivalSoilForceCal() {
	
	size_t ID;
	double fv_temp_sum = 0, fv_temp_sum_E = 0;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		double width,max_width = 0.0;
		fv_temp_sum = Var->LevelSection[i].Level_sum_Fh*(tan(15 * M_PI / 180));
		fv_temp_sum_E = Var->LevelSection[i].Level_sum_Fh_E*(tan(15 * M_PI / 180));
		Var->LevelSection[i].Level_Fv_sum = fv_temp_sum;
		Var->LevelSection[i].Level_Fv_sum_E = fv_temp_sum_E;
		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++) {
			ID = Var->LevelSection[i].BlockId[j];
			width = Var->BlockData[ID].MaxX - Var->BlockData[ID].MinX;
			if (Var->BlockData[ID].CalMoment == true && width > max_width) {
				max_width = width;
			}
		}
		Var->LevelSection[i].Level_Fv_x = max_width;
		Var->LevelSection[i].Level_Fv_x_E = max_width;
		Var->LevelSection[i].Level_Fv_Mv_sum = Var->LevelSection[i].Level_Fv_sum*Var->LevelSection[i].Level_Fv_x;
		Var->LevelSection[i].Level_Fv_Mv_sum_E = Var->LevelSection[i].Level_Fv_sum_E*Var->LevelSection[i].Level_Fv_x_E;
	}
	Var->Err_Msg += "土壓垂直力及抵抗彎矩計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::ResidualWaterForceCal() {
	double Pw;
	Pw = Var->rw*(Var->RWL - Var->LWL);
	double top_level = Var->Max_level;
	double arm_temp,Fw_temp;
	double temp_sum_Fw = 0;
	double temp_sum_FwMw = 0;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		

		if (Var->LevelSection[i].Level < Var->RWL) 
		{
			if (i >= 1 && temp_sum_Fw > 0) {
				Var->LevelSection[i].pre_sum_Fw = Var->LevelSection[i - 1].Level_sum_Fw;
				double Arm_len = top_level - Var->LevelSection[i].Level;
				Var->LevelSection[i].pre_total_Fwy = Var->LevelSection[i - 1].Level_total_Fwy + Arm_len;
				Var->LevelSection[i].pre_sum_FwMw = Var->LevelSection[i].pre_sum_Fw*Var->LevelSection[i].pre_total_Fwy;
				if (Var->LevelSection[i].pre_sum_Fw > 0) {
					temp_sum_Fw = Var->LevelSection[i].pre_sum_Fw;
					temp_sum_FwMw = Var->LevelSection[i].pre_sum_FwMw;
				}
			}

			if (top_level > Var->RWL) {
				top_level = Var->RWL;
				if (top_level > Var->LWL) {
					Var->LevelSection[i].Fw_sum = (top_level - Var->LWL)*0.5*Pw;

					arm_temp = (top_level - Var->LevelSection[i].Level) - ((top_level - Var->LWL) * 2 / 3);
					Var->LevelSection[i].Fw_Mw_sum = Var->LevelSection[i].Fw_sum*arm_temp;

					Fw_temp = (Var->LWL - Var->LevelSection[i].Level)*Pw;
					Var->LevelSection[i].Fw_sum += Fw_temp;

					arm_temp = (Var->LWL - Var->LevelSection[i].Level)/2;
					Var->LevelSection[i].Fw_Mw_sum += Fw_temp*arm_temp;

					Var->LevelSection[i].Fw_y = Var->LevelSection[i].Fw_Mw_sum / Var->LevelSection[i].Fw_sum;
					
					//summation
					temp_sum_Fw += Var->LevelSection[i].Fw_sum;
					temp_sum_FwMw += Var->LevelSection[i].Fw_Mw_sum;
				}
				else {
					Var->LevelSection[i].Fw_sum = (top_level - Var->LevelSection[i].Level)*Pw;

					arm_temp = (top_level - Var->LevelSection[i].Level) / 2;
					Var->LevelSection[i].Fw_Mw_sum = Var->LevelSection[i].Fw_sum*arm_temp;

					Var->LevelSection[i].Fw_y = Var->LevelSection[i].Fw_Mw_sum / Var->LevelSection[i].Fw_sum;

					//summation
					temp_sum_Fw += Var->LevelSection[i].Fw_sum;
					temp_sum_FwMw += Var->LevelSection[i].Fw_Mw_sum;
				}
			}
			else {
				if (top_level > Var->LWL) {
					Var->LevelSection[i].Fw_sum = (top_level - Var->LWL)*0.5*Pw;

					arm_temp = (top_level - Var->LevelSection[i].Level) - ((top_level - Var->LWL) * 2 / 3);
					Var->LevelSection[i].Fw_Mw_sum = Var->LevelSection[i].Fw_sum*arm_temp;

					Var->LevelSection[i].Fw_sum += (Var->LWL - Var->LevelSection[i].Level)*Pw;

					arm_temp = (Var->LWL - Var->LevelSection[i].Level) / 2;
					Var->LevelSection[i].Fw_Mw_sum += Var->LevelSection[i].Fw_sum*arm_temp;

					Var->LevelSection[i].Fw_y = Var->LevelSection[i].Fw_Mw_sum / Var->LevelSection[i].Fw_sum;

					//summation
					temp_sum_Fw += Var->LevelSection[i].Fw_sum;
					temp_sum_FwMw += Var->LevelSection[i].Fw_Mw_sum;
				}
				else {
					Var->LevelSection[i].Fw_sum = (top_level - Var->LevelSection[i].Level)*Pw;

					arm_temp = (top_level - Var->LevelSection[i].Level) / 2;
					Var->LevelSection[i].Fw_Mw_sum = Var->LevelSection[i].Fw_sum*arm_temp;

					Var->LevelSection[i].Fw_y = Var->LevelSection[i].Fw_Mw_sum / Var->LevelSection[i].Fw_sum;
					
					//summation
					temp_sum_Fw += Var->LevelSection[i].Fw_sum;
					temp_sum_FwMw += Var->LevelSection[i].Fw_Mw_sum;
				}
			}
		}
		else {
			Var->LevelSection[i].Fw_sum = 0;
			Var->LevelSection[i].Fw_Mw_sum = 0;
			Var->LevelSection[i].Fw_y = 0;

			//summation
			temp_sum_Fw += Var->LevelSection[i].Fw_sum;
			temp_sum_FwMw += Var->LevelSection[i].Fw_Mw_sum;
		}

		top_level = Var->LevelSection[i].Level;

		Var->LevelSection[i].Level_sum_Fw = temp_sum_Fw;
		Var->LevelSection[i].Level_sum_FwMw = temp_sum_FwMw;

		Var->LevelSection[i].Level_total_Fwy = Var->LevelSection[i].Level_sum_FwMw / Var->LevelSection[i].Level_sum_Fw;

	}

	Var->Err_Msg += "殘留水壓及傾倒彎矩計算處理完畢! \r\n";
	return true;
}


bool Module2_Internal::ShipTractionForceCal() {

	double top_level = Var->Max_level;
	
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		Var->LevelSection[i].Ft_y = (top_level - Var->LevelSection[i].Level) + Var->hd;

		Var->LevelSection[i].Ft_Mt = Var->Ta*Var->LevelSection[i].Ft_y;
		//top_level = Var->LevelSection[i].Level;
	}


	Var->Err_Msg += "船舶牽引力及傾倒彎矩計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::VerticalForceSum() {
	for (size_t i = 0; i < Var->LevelSection.size(); i++){

		Var->LevelSection[i].VForcesum = Var->LevelSection[i].Level_sum_W + Var->LevelSection[i].Level_Fv_sum;
		Var->LevelSection[i].VMomentsum = Var->LevelSection[i].Level_sum_Mx + Var->LevelSection[i].Level_Fv_Mv_sum;

		Var->LevelSection[i].VForcesum_E = Var->LevelSection[i].Level_sum_W + Var->LevelSection[i].Level_Fv_sum_E;
		Var->LevelSection[i].VMomentsum_E = Var->LevelSection[i].Level_sum_Mx + Var->LevelSection[i].Level_Fv_Mv_sum_E;
	}

	Var->Err_Msg += "垂直力及抵抗彎矩總合表計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::HorizontalForceSum() {
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {

		Var->LevelSection[i].HForcesum = Var->LevelSection[i].Level_sum_Fh + Var->Ta + Var->LevelSection[i].Level_sum_Fw;
		Var->LevelSection[i].HMomentsum = Var->LevelSection[i].Level_sum_FhMh + Var->LevelSection[i].Ft_Mt + Var->LevelSection[i].Level_sum_FwMw;

		Var->LevelSection[i].HForcesum_E = Var->LevelSection[i].Level_sum_Fh_E + Var->LevelSection[i].Level_sum_WE + Var->LevelSection[i].Level_sum_Fw;
		Var->LevelSection[i].HMomentsum_E = Var->LevelSection[i].Level_sum_FhMh_E + Var->LevelSection[i].Level_sum_MxE + Var->LevelSection[i].Level_sum_FwMw;
	}

	Var->Err_Msg += "水平力及傾倒彎矩總合表計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::SafetyFactorCheck() {
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		Var->LevelSection[i].SF_slide = (Var->LevelSection[i].VForcesum / Var->LevelSection[i].HForcesum);
		Var->LevelSection[i].SF_slide_E = Var->LevelSection[i].VMomentsum / Var->LevelSection[i].HMomentsum;
		Var->LevelSection[i].SF_overturning = (Var->LevelSection[i].VForcesum_E / Var->LevelSection[i].HForcesum_E);
		Var->LevelSection[i].SF_overturning_E = Var->LevelSection[i].VMomentsum_E / Var->LevelSection[i].HMomentsum_E;
	}
	Var->Err_Msg += "壁體安全檢核計算處理完畢! \r\n";
	return true;
}