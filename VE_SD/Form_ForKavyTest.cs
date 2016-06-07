using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VE_SD
{
    public partial class Form_ForKavyTest : Form
    {
        public Form_ForKavyTest()
        {
            InitializeComponent();
            // Class New
            Module1 Mod = new Module1();

            ////Block API
            ////- New Block
            //int BlockId = Mod.NewBlock(3.0, 2.0);
            //MessageBox.Show(BlockId.ToString());
            ////- Add Coord
            //Mod.SetBlockCoord(BlockId, 3.5, 4.0);
            //Mod.SetBlockCoord(BlockId, 7.0, 4.3);
            //Mod.SetBlockCoord(BlockId, 5.2, 1.3);
            ////- Test Delete Wrong Block ID
            //BlockId = 2;
            //if (Mod.DeleteBlock(BlockId))
            //{
            //    MessageBox.Show("已刪除!");
            //}
            //else { MessageBox.Show("錯誤ID!"); }
            ////- Add New
            //BlockId = Mod.NewBlock(3.7, 2.7);
            //MessageBox.Show(Mod.GetNumOfBlock().ToString());
            //MessageBox.Show(Mod.ErrMsg);
            ////- Delete All Block
            //if (Mod.DeleteAllBlockData())
            //{
            //    MessageBox.Show("已刪除All Block!");
            //}

            //// Class Main Run
            //Mod.Run();

            //// Class output test
            //double org = 0.0;
            //Mod.VarOut(ref org);
            //MessageBox.Show(org.ToString());

            ////Out Log File
            //String Pos = "C:\\Users\\kavy\\Desktop\\test.log";
            //Mod.OutPutLogFile(Pos);

            //// Class Memory Clear
            //Mod.Dispose();

            //// Class Mesg output
            //textBox1.Text = Mod.ErrMsg;



        }
    }
}

