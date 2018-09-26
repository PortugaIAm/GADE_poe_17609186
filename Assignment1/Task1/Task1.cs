using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assignment1
{
    // This program forms the bases to a RTS game that will run in unity. The theme of the game is war between two teams.


    public partial class Form1 : Form
    {
        // global variables and instance of objects
        int tick;
        public string t1Name= "team1",t2Name= "team2";
        public int team1rU1=5, team1rU2=5, team1mU1=5, team1mU2=5, team2rU1=5, team2rU2=5, team2mU1=5, team2mU2=5;
        public bool running = false;
        MeleeUnit meleeUnit;
        RangedUnit rangedUnit;
        emptyTile eT = new emptyTile();
        ResourceBuilding RB1 = new ResourceBuilding();
        ResourceBuilding RB2 = new ResourceBuilding();
        ResourceBuilding[] allRB = new ResourceBuilding[0];
        Map field;
        Map map;
        GameEngine gm;



        public Form1()
        {

            InitializeComponent();
            timer1.Stop();
            field = new Map(this.gridM, this, RB1, eT);
            gm = new GameEngine(this);
        }

        //classes

        // All units in the game 
        // Base Class
        public abstract class Unit : PictureBox
        {
            //Fields
            protected int XPos;
            protected int YPos;
            protected int HP;
            protected int Speed;
            protected int Atk;
            protected int AtkRange;
            protected string Fraction;
            protected string Image;
            protected string uName;
            public Unit()
            {
                //A constructor
            }
            public virtual void Move()
            {
                //A method to move to a new position
            }
            public virtual void Attack()
            {

                //A method to handle combat with another unit
            }
            public virtual bool CanAttack()
            {
                //A method to determine whether another unit is within attack range
                return false;
            }
            public virtual void PosClosestUnit()
            {
                //A method to return position of the closest other unit to this unit
            }
            public virtual void Death()
            {
                //A method to handle the death of this unit
            }
           /* public override virtual string ToString()
            {
                //Returns a string showing all the unit information
                return null;
            */
            public virtual void Save()
            {
                //method used to save any units postion
            }
        }

        // This class serves as an empty object for the map array to avoiding having nulls 
        public class emptyTile : PictureBox
        {
            int xPos;
            int yPos;
            public int XPos
            {
                get { return xPos; }
                set { xPos = value; }
            }
            public int YPos
            {
                get { return yPos; }
                set { yPos = value; }
            }
            public emptyTile()
            {

            }
            public override string ToString()
            {
                string s = "empty Tile" + Environment.NewLine + "(" + xPos + "," + yPos + ")";
                return s;
            }
        }

        // This is the melee unit 
        public class MeleeUnit : Unit  
        {
            Form1 fm;
            protected int xPos;
            protected int yPos;
            protected int hP = 100;
            protected int speed = 1;
            protected int atk = 20;
            protected int atkRange = 1;
            protected string fraction;
            protected string image = "MeleeUnit.png";
            protected string uName;
            int distance = 100;
            int cX, cY;
            int NxPos, NyPos;
            Map map;
            emptyTile eT = new emptyTile();

           // refrences
           //Image Refrence accessed on[13 / 08 / 2018]
           //Image Name :Rebel Soldier - Metal Slug
           // Image Created by nickstudios20
           // https://www.google.co.za/search?q=pixel+art+characters&tbm=isch&tbs=rimg:Ca0vwltQjdOAIjgWfmW7eBv26revJ4rGQAQl9b4ZiS6EjWUIFPI0Xy4XcAtxrdoNC7xzLK9c8E94Ku3jcpzVmFVJnCoSCRZ-Zbt4G_1bqEa4JC0QqQou0KhIJt68nisZABCURc-mdDxC6ljAqEgn1vhmJLoSNZREEAlLLPXrs3yoSCQgU8jRfLhdwEXXKev0jvmMPKhIJC3Gt2g0LvHMR0FB1NR6eY7cqEgksr1zwT3gq7RHWgLuSo3KXoyoSCeNynNWYVUmcESMtBkQRYMPU&tbo=u&sa=X&ved=2ahUKEwioh4Ht-uncAhUyx4UKHeZcAasQ9C96BAgBEBs&biw=1229&bih=568&dpr=1.56#imgrc=qbcbwaMAsHegAM:

           // Image Refrence accessed on[25 / 09 / 2018]
           // Image Name :Rebel Soldier(Shield)
           // Image Created by Gussprint
           // https://www.google.co.za/search?q=soldier&tbm=isch&tbs=rimg:CbNR0i7i8a3OIjjQ9ud9A-0YGpgRDpZ_1W6a652awyusxFHzC5n2GyHJDNGAXop9AzvEzzVGr8k4jhvzxvyvocM0W8yoSCdD2530D7RgaEWc_1Ju6E0Cj1KhIJmBEOln9bproRiArHjCT0W5YqEgnnZrDK6zEUfBES1Sf6CsHBFSoSCcLmfYbIckM0ETkg3hJF06HDKhIJYBein0DO8TMRhRUlLGJhYBYqEgnNUavyTiOG_1BHne46VMkTmTCoSCfG_1K-hwzRbzEWTS_1xIR2u2_1&tbo=u&sa=X&ved=2ahUKEwjgm778xdbdAhUKBMAKHQuzCkoQ9C96BAgBEBs&biw=1229&bih=603&dpr=1.56#imgrc=PwoSdEl7qg3PCM:


            //methods

            public MeleeUnit(Map map, Form1 fm)
            {
                this.fm = fm;
                this.map = map;
            }

            
            public override void Move()
            {
                //fm.gridM.SuspendLayout();
                //fm.gridM.Controls.Clear();
                if (HP > HP * 0.25)
                {
                    
                    //map.arrPb[xPos, yPos] = new ET();//.Name = "E";

                    if (xPos != cX && yPos == cY)
                    {
                        if (xPos < cX)
                        {
                            NxPos = xPos + 1;
                            if (map.arrPb[NxPos, yPos].Name == "E")
                            { xPos = NxPos; }
                        }
                        else if (xPos > cX)
                        {
                            NxPos = xPos - 1;
                            if (map.arrPb[NxPos, yPos].Name == "E")
                            { xPos = NxPos; }
                        }
                    }
                    else if (yPos != cY && xPos == cX)
                    {
                        if (yPos < cY)
                        {
                            NyPos = yPos + 1 + speed;
                            if (map.arrPb[xPos, NyPos].Name == "E")
                            { yPos = NyPos; }
                        }
                        else if (yPos > cY)
                        {
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[xPos, NyPos].Name == "E")
                            { yPos = NyPos; }
                        }
                    }
                    else if (xPos != cX && yPos != cY)
                    {
                        if (xPos < cX && yPos < cY)
                        {
                            NxPos = xPos + 1 + speed;
                            NyPos = yPos + 1 + speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                        else if (xPos > cX && yPos > cY)
                        {
                            NxPos = xPos - 1 - speed;
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                        else if (xPos < cX && yPos > cY)
                        {
                            NxPos = xPos + 1 + speed;
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                        else //(xPos > cX && yPos < cY )
                        {
                            NxPos = xPos - 1 - speed;
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                    }
                    map.arrPb[xPos, yPos] = new emptyTile();
                    //map.arrPb[xPos, yPos].Name = "M";
                    map.placeUnits();
                    //fm.gridM.Refresh();
                    //fm.gridM.ResumeLayout();

                }
                else
                {

                }
            }
            
            public override void PosClosestUnit()
            {
                if (map.arrPb[XPos, YPos].Name == "M" || map.arrPb[XPos, YPos].Name == "R")
                {
                    for (int a = 0; a < 20; a++)
                    {
                        for (int k = 0; k < 20; k++)
                        {
                            if (map.arrPb[a, k].Name == "M" || map.arrPb[a, k].Name == "R" && map.arrPb[a, k] == map.arrPb[XPos, YPos])
                            {
                                double r = Math.Sqrt((a * XPos) + (YPos * k));
                                if (r < distance)
                                {
                                    cX = a;
                                    cY = k;
                                    distance = Convert.ToInt32(r);
                                }
                            }
                        }
                    }
                }

                CanAttack();
            }
            
            public override bool CanAttack()
            {
                if (distance <= atkRange)
                {
                    return true;
                }
                else return false;
            }
            
            public override void Attack()
            {
                if (CanAttack())
                {

                    //attack
                    // if attack by slug
                    if (map.arrPb[cX, cY].Name == "M1")
                    {
                        hP = hP - 20;
                    }
                    // if attacked by axel
                    else if (map.arrPb[cX, cY].Name == "M2")
                    {
                        hP = hP - 30;
                    }
                    // if attacked by pablitos
                    else if (map.arrPb[cX, cY].Name == "rU1")
                    {
                        hP = hP - 10;
                    }
                    // if attacked by boom boom guy
                    else
                    {
                        hP = hP - 50;
                    }
                    Move();
                }
                else { Move(); }
                Death();
            }
            
            public override void Death()
            {
                map.arrPb[xPos, yPos].Name = "E";
                map.arrPb[xPos, yPos] = eT;
            }
            
            public override string ToString()
            {
                string str;
                str = "Melee Unit" + Environment.NewLine + "Name: " + UName + Environment.NewLine + "Team: " + fraction + Environment.NewLine + "(" + xPos + "," + yPos + ")" + Environment.NewLine + "Health: " + hP + Environment.NewLine + "Attack: " + atk + Environment.NewLine + "Speed: " + speed;
                return str;
            }
            
            public override void Save()
            {

            }

            //gets values
            public int XPos
            {
                get { return xPos; }
                set { xPos = value; }
            }
            public int YPos
            {
                get { return yPos; }
                set { yPos = value; }
            }
            public int HP
            {
                get { return hP; }
                set { hP = value; }
            }
            public int Speed
            {
                get { return speed; }
                set { speed = value; }
            }
            public int Atk
            {
                get { return atk; }
                set { atk = value; }
            }
            public int AtkRange
            {
                get { return atkRange; }
                set { atkRange = value; }
            }
            public string Fraction
            {
                get { return fraction; }
                set { fraction = value; }
            }
            public string UName
            {
                get { return uName; }
                set { uName = value; }
            }
        }

        // This is the ranged unit
        public class RangedUnit : Unit  //unit that can only attack other units up to its attack range
        {
            Form1 fm;
            public int xPos;
            public int yPos;
            public int hP = 50;
            public int speed;
            public int atk = 10;
            private int atkRange = 5;
            private string fraction;
            private string image = "RangedUnit.png";
            int distance = 100;
            private string uName;
            int cX, cY,NxPos,NyPos;
            Map map;
            emptyTile eT = new emptyTile();

            //refrence
            //Image Refrence accessed on[13 / 08 / 2018]
            //Image Name :Metal Slug Soldier | Pixel Art Maker
            //Image Created by Pixel Art Maker
            //https://www.google.co.za/search?q=pixel+art+characters&tbm=isch&tbs=rimg:Ca0vwltQjdOAIjgWfmW7eBv26revJ4rGQAQl9b4ZiS6EjWUIFPI0Xy4XcAtxrdoNC7xzLK9c8E94Ku3jcpzVmFVJnCoSCRZ-Zbt4G_1bqEa4JC0QqQou0KhIJt68nisZABCURc-mdDxC6ljAqEgn1vhmJLoSNZREEAlLLPXrs3yoSCQgU8jRfLhdwEXXKev0jvmMPKhIJC3Gt2g0LvHMR0FB1NR6eY7cqEgksr1zwT3gq7RHWgLuSo3KXoyoSCeNynNWYVUmcESMtBkQRYMPU&tbo=u&sa=X&ved=2ahUKEwioh4Ht-uncAhUyx4UKHeZcAasQ9C96BAgBEBs&biw=1229&bih=568&dpr=1.56#imgrc=DvW8PlLN_K4dqM:

            //Image Refrence accessed on[25 / 09 / 2018]
            //Image Name :Rebel Soldier(Bazooka)
            //Image Created by Gussprint
            //https://www.google.co.za/search?q=soldier&tbm=isch&tbs=rimg:CbNR0i7i8a3OIjjQ9ud9A-0YGpgRDpZ_1W6a652awyusxFHzC5n2GyHJDNGAXop9AzvEzzVGr8k4jhvzxvyvocM0W8yoSCdD2530D7RgaEWc_1Ju6E0Cj1KhIJmBEOln9bproRiArHjCT0W5YqEgnnZrDK6zEUfBES1Sf6CsHBFSoSCcLmfYbIckM0ETkg3hJF06HDKhIJYBein0DO8TMRhRUlLGJhYBYqEgnNUavyTiOG_1BHne46VMkTmTCoSCfG_1K-hwzRbzEWTS_1xIR2u2_1&tbo=u&sa=X&ved=2ahUKEwjgm778xdbdAhUKBMAKHQuzCkoQ9C96BAgBEBs&biw=1229&bih=603&dpr=1.56#imgrc=m-XHAZcleYKzQM:


            //methods

            public RangedUnit(Map map, Form1 fm)
            {
                this.fm = fm;
                this.map = map;
            }

            // This method moves the unit along the grid
            public override void Move()
            {
                
                if (HP > HP * 0.25)
                {
                    fm.gridM.SuspendLayout();
                    //fm.gridM.Controls.Clear();
                    map.arrPb[xPos, yPos].Name = "E";

                    if (xPos != cX && yPos == cY)
                    {
                        if (xPos < cX)
                        {
                            NxPos = xPos + 1;
                            if (map.arrPb[NxPos, yPos].Name == "E")
                            { xPos = NxPos; }
                        }
                        else if (xPos > cX)
                        {
                            NxPos = xPos - 1;
                            if (map.arrPb[NxPos, yPos].Name == "E")
                            { xPos = NxPos; }
                        }
                    }
                    else if (yPos != cY && xPos == cX)
                    {
                        if (yPos < cY)
                        {
                            NyPos = yPos + 1 + speed;
                            if (map.arrPb[xPos, NyPos].Name == "E")
                            { yPos = NyPos; }
                        }
                        else if (yPos > cY)
                        {
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[xPos, NyPos].Name == "E")
                            { yPos = NyPos; }
                        }
                    }
                    else if (xPos != cX && yPos != cY)
                    {
                        if (xPos < cX && yPos < cY)
                        {
                            NxPos = xPos + 1 + speed;
                            NyPos = yPos + 1 + speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                        else if (xPos > cX && yPos > cY)
                        {
                            NxPos = xPos - 1 - speed;
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                        else if (xPos < cX && yPos > cY)
                        {
                            NxPos = xPos + 1 + speed;
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                        else //(xPos > cX && yPos < cY )
                        {
                            NxPos = xPos - 1 - speed;
                            NyPos = yPos - 1 - speed;
                            if (map.arrPb[NxPos, NyPos].Name == "E")
                            {
                                yPos = NyPos;
                                xPos = NxPos;
                            }
                        }
                    }

                    map.arrPb[xPos, yPos].Name = "M";
                    fm.gridM.Refresh();
                    map.placeUnits();
                    fm.gridM.ResumeLayout();
                }
                else
                {

                }

            }
            // This method finds the position and distance of the closest unit on the map to the current unit
            public override void PosClosestUnit()
            {
                if (map.arrPb[XPos, YPos].Name == "M" || map.arrPb[XPos, YPos].Name == "R")
                {
                    for (int a = 0; a < 20; a++)
                    {
                        for (int k = 0; k < 20; k++)
                        {
                            if (map.arrPb[a, k].Name == "M" || map.arrPb[a, k].Name == "R" && map.arrPb[a, k] == map.arrPb[XPos, YPos])
                            {
                                double r = Math.Sqrt((a * XPos) + (YPos * k));
                                if (r < distance)
                                {
                                    cX = a;
                                    cY = k;
                                    distance = Convert.ToInt32(r);
                                }
                            }
                        }
                    }
                }
                CanAttack();
            }
            // This method checks if the closest unit is within range and returns a bool
            public override bool CanAttack()
            {
                if (distance <= atkRange)
                {
                    return true;
                }
                else return false;

            }
            // This method handles the attacking of 2 units
            public override void Attack()
            {
                if (CanAttack())
                {

                    //attack
                    // if attack by slug
                    if (map.arrPb[cX, cY].Name == "M1")
                    {
                        hP = hP - 20;
                    }
                    // if attacked by axel
                    else if (map.arrPb[cX, cY].Name == "M2")
                    {
                        hP = hP - 30;
                    }
                    // if attacked by pablitos
                    else if (map.arrPb[cX, cY].Name == "rU1")
                    {
                        hP = hP - 10;
                    }
                    // if attacked by boom boom guy
                    else
                    {
                        hP = hP - 50;
                    }
                    Move();
                }
                else { Move(); }
                Death();
            }
            // This method handles the attacking of 2 units           
            public override void Death()
            {
                map.arrPb[xPos, yPos].Name = "E";
                map.arrPb[xPos, yPos] = eT;
            }
            // This method handles the death of a unit
            public override string ToString()
            {
                string str;
                str = "Ranged Unit" + Environment.NewLine + "Name: " + UName + Environment.NewLine + "Team: " + fraction + Environment.NewLine + "(" + xPos + "," + yPos + ")" + Environment.NewLine + "Health: " + hP + Environment.NewLine + "Attack: " + atk + Environment.NewLine + "Speed: " + speed;
                return str;
            }
            // This displays all the info regarding the unit
            public override void Save()
            {

            }

            //gets values
            public int XPos
            {
                get { return xPos; }
                set { xPos = value; }
            }
            public int YPos
            {
                get { return yPos; }
                set { yPos = value; }
            }
            public int HP
            {
                get { return hP; }
                set { hP = value; }
            }
            public int Speed
            {
                get { return speed; }
                set { speed = value; }
            }
            public int Atk
            {
                get { return atk; }
                set { atk = value; }
            }
            public int AtkRange
            {
                get { return atkRange; }
                set { atkRange = value; }
            }
            public string Fraction
            {
                get { return fraction; }
                set { fraction = value; }
            }
            public string UName
            {
                get { return uName; }
                set { uName = value; }
            }

        }

        // All buildings
        // Base Clase
        public class Building : PictureBox
        {
            //Fields
            protected string Name;
            protected int xPos;
            protected int yPos;
            protected int hP = 100;
            protected string fraction;
            protected string image = "Factory.png";

            //methods
            public Building()
            {
                //Constructer to the builder class
            }
            public virtual void Death()
            {
                //A method to handle the death of this unit
            }
            public virtual string ToString()
            {
                //Returns a string showing all the unit information
                return null;
            }
            public virtual void Save()
            { }
        }

        // The Resource Building generates gold per sec to be used by the Factory Building
        public class ResourceBuilding : Building
        {
            Form1 fm;
            private string resType;
            private int resPerTick = 2;
            private int resCount = 0;


            //Image Refrence accessed on [04/09/2018]
            //Image Name :Brazil prosecutors demand crackdown on illegal gold mining in Amazon's El Dorado
            //Image Created by Unknown
            //https://steelguru.com/mining/brazil-prosecutors-demand-crackdown-on-illegal-gold-mining-in-amazon-s-el-dorado/480249 

            //methods
            public void GenerateRes()
            {
                resCount += resPerTick;
            }
            public override void Death()
            {
                //A method to handle the death of this unit
            }
            public override string ToString()
            {
                string str = "Resource Building" + Environment.NewLine + "Fraction: " + fraction + "(" + xPos + "," + yPos + ")" + Environment.NewLine + "Health: " + hP + Environment.NewLine + "Resource: " + resType + " " + resCount;
                return str;
            }

            //gets the values
            public int XPos
            {
                get { return xPos; }
                set { xPos = value; }
            }
            public int YPos
            {
                get { return yPos; }
                set { yPos = value; }
            }
            public int HP
            {
                get { return hP; }
                set { hP = value; }
            }
            public int ResCount
            {
                get { return resCount; }
                set { resCount = value; }
            }
            public string Fraction
            {
                get { return fraction; }
                set { fraction = value; }
            }

        }

        // The Factory creates units for both teams based on the gold generated by the Resource building
        public class Factory : Building
        {
            Form1 fm;
            Map map;
            ResourceBuilding rb;
            MeleeUnit mU;
            protected int gameTick;
            private int spawnX;
            private int spawnY;
            public static BinaryFormatter BFormatter { get; set; }
            public static FileStream FStream { get; set; }

            //Image Refrence accessed on [04/09/2018]
            //Image Name : Factory Plant Machine
            //Image Created by Clker-Free-Vector-Images
            //https://pixabay.com/en/factory-plant-machine-manufacturing-40621/

            //methods
            public Factory(Map map, Form1 fm, ResourceBuilding rb, MeleeUnit mU)
            {
                this.map = map;
                this.fm = fm;
                this.rb = rb;
                this.mU = mU;
                
            }
            public void Spawn()
            {
                if (rb.ResCount == 10)
                {
                    // i just tried to get the map to spawn melee units but thde code didnt seem to work.
                    spawnX = XPos + 1;
                    spawnY = YPos + 1;
                    if (map.arrPb[spawnX,spawnY].Name == "E")
                    {
                        MeleeUnit newMu = new MeleeUnit(map, fm);
                        newMu.XPos = spawnX;
                        newMu.XPos = spawnY;
                        newMu.Fraction = fraction;
                        map.arrPb[spawnX, spawnY] = newMu;
                        map.placeUnits();
                    }
                    fm.gridM.Refresh();
                }
            }
            public override void Death()
            {
                //A method to handle the death of this unit
            }
            public override string ToString()
            {
                string str;
                str = "Factory Building" + Environment.NewLine + "Fraction: " + fraction + Environment.NewLine + "(" + xPos + "," + yPos + ")" + Environment.NewLine + "Health: " + hP;
                return str;
            }
            public static void Save(string FileName, Factory fB)
            {
                FStream = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                BFormatter = new BinaryFormatter();
                BFormatter.Serialize(FStream, fB);
                FStream.Close();
            }
            public static Factory[] Load(string FileName, Factory fb)
            {
                FStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BFormatter = new BinaryFormatter();
                Factory[] facArray = new Factory[0];
                int i = 0;

                while (FStream.Position < FStream.Length)
                {
                    Array.Resize(ref facArray, facArray.Length + 1);
                    facArray[i] = (Factory)BFormatter.Deserialize(FStream);
                    i++;
                }
                FStream.Close();
                return facArray;
            }

            //gets the values
            public int XPos
            {
                get { return xPos; }
                set { xPos = value; }
            }
            public int YPos
            {
                get { return yPos; }
                set { yPos = value; }
            }
            public int HP
            {
                get { return hP; }
                set { hP = value; }
            }
            public string Fraction
            {
                get { return fraction; }
                set { fraction = value; }
            }
        }

        // The map class generate the map and is used to save and load the game at any point in time
        [Serializable]
        public class Map
        {
            TableLayoutPanel gridM;
            Form1 main;
            ResourceBuilding rb;
            emptyTile eT;

            //Fields
            public string t1 = "team1";
            public string t2 = "team2";
            int T1mUnitnum1=5;
            int T1mUnitnum2=5;
            int T1rUnitnum1=5;
            int T1rUnitnum2=5;

            int T2mUnitnum1=5;
            int T2mUnitnum2=5;
            int T2rUnitnum1=5;
            int T2rUnitnum2=5;

            public int T1MUnitnum1
            {
                get { return T1mUnitnum1; }
                set { T1mUnitnum1 = value; }
            }
            public int T1MUnitnum2
            {
                get { return T1mUnitnum2; }
                set { T1mUnitnum2 = value; }
            }
            public int T1RUnitnum1
            {
                get { return T1rUnitnum1; }
                set { T1rUnitnum1 = value; }
            }
            public int T1RUnitnum2
            {
                get { return T1rUnitnum2; }
                set { T1rUnitnum2 = value; }
            }
            public int T2MUnitnum1
            {
                get { return T2mUnitnum1; }
                set { T2mUnitnum1 = value; }
            }
            public int T2MUnitnum2
            {
                get { return T2mUnitnum2; }
                set { T1mUnitnum2 = value; }
            }
            public int T2RUnitnum1
            {
                get { return T2rUnitnum1; }
                set { T2rUnitnum1 = value; }
            }
            public int T2RUnitnum2
            {
                get { return T2rUnitnum2; }
                set { T2rUnitnum2 = value; }
            }

            public static BinaryFormatter BFormatter { get; set; }
            public static FileStream FStream { get; set; }

            public PictureBox [,] arrPb =new PictureBox [20,20];

            //generates an array for unit location tracking
            public Map(TableLayoutPanel gridM, Form1 main, ResourceBuilding rb, emptyTile eT)
            {
                this.gridM = gridM;
                this.main = main;
                this.rb = rb;
                this.eT = eT;
            }

            //generates units and buildng randomly
            public void GenerateUnits()
            {
                // E represents empty tile
                // M represents a melee unit in the map
                // M1 normal soldier M2 is a swordsman
                // R represemts a ranged unit in the map
                // R1 is a rifelman
                // R2 is a missile launcher

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        emptyTile eT1 = new emptyTile();
                        eT1.Name = "E";
                        eT1.XPos = i;
                        eT1.YPos = j;
                        arrPb[i, j] = eT1;
                    }
                }

                MeleeUnit mU = new MeleeUnit(this,main);
                main.meleeUnit = mU;
                RangedUnit rU = new RangedUnit(this,main);
                main.rangedUnit = rU;
                ResourceBuilding RB = new ResourceBuilding();
                Factory FB = new Factory(this,main ,RB,mU);
                
                //team 1 units
                // This populates the map with melee unit one, slugs
                for (int i = 0; i < T1mUnitnum1; i++)
                {
                    MeleeUnit mU1 = new MeleeUnit(this,main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    mU1.Name = "M1";
                    mU1.UName = "Slug";
                    mU1.XPos = x;
                    mU1.YPos = y;
                    mU1.Fraction = t1;
                    arrPb[x, y] = mU1;

                }
                // This populates the map with melee unit two, swordman
                for (int i = 0; i < T1mUnitnum2; i++)
                {
                    MeleeUnit mU1 = new MeleeUnit(this, main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    mU1.Name = "M2";
                    mU1.UName = "Axel";
                    mU1.HP = 80;
                    mU1.Speed = 0;
                    mU1.Atk = 30;
                    mU1.XPos = x;
                    mU1.YPos = y;
                    mU1.Fraction = t1;
                    arrPb[x, y] = mU1;
                }
                // This populates the map with ranged Unit one, rifleman
                for (int i = 0; i < T1rUnitnum1; i++)
                {
                    RangedUnit rU1 = new RangedUnit(this,main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);

                    while (arrPb[x, y] == mU)
                    {
                        rnd = new Random(Guid.NewGuid().GetHashCode());
                        rnd2 = new Random(Guid.NewGuid().GetHashCode());
                        x = rnd.Next(0, 20);
                        y = rnd2.Next(0, 20);
                    }
                    rU1.Name = "rU1";
                    rU1.UName = "Pablitos";
                    rU1.XPos = x;
                    rU1.YPos = y;
                    rU1.Fraction = t1;
                    arrPb[x, y] = rU1;
                }
                // This populates the map with ranged Unit two, missile launcher
                for (int i = 0; i < T1rUnitnum2; i++)
                {
                    RangedUnit rU1 = new RangedUnit(this, main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);

                    while (arrPb[x, y] == mU)
                    {
                        rnd = new Random(Guid.NewGuid().GetHashCode());
                        rnd2 = new Random(Guid.NewGuid().GetHashCode());
                        x = rnd.Next(0, 20);
                        y = rnd2.Next(0, 20);
                    }
                    rU1.Name = "rU2";
                    rU1.UName = "Boom Boom Guy";
                    rU1.atk = 30;
                    rU1.AtkRange = 3;
                    rU1.XPos = x;
                    rU1.YPos = y;
                    rU1.Fraction = t1;
                    arrPb[x, y] = rU1;
                }
                // This populates the map with Resource Buildings
                for (int i = 0; i < 1; i++)
                {
                    ResourceBuilding RB1 = new ResourceBuilding();
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    RB1.Name = "RB";
                    RB1.XPos = x;
                    RB1.YPos = y;
                    RB1.Fraction = " 1";
                    RB1.Name = "RB";
                    arrPb[x, y] = RB1;
                }
                // This populates the map with Factory Buildings
                for (int i = 0; i < 1; i++)
                {
                    Factory FB1 = new Factory(this, main, RB, mU);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    FB1.Name = "FB";
                    FB1.XPos = x;
                    FB1.YPos = y;
                    FB1.Fraction = " 1";
                    arrPb[x, y] = FB1;
                }

                //team 2units
                // This populates the map with melee unit one, slugs
                for (int i = 0; i < T2mUnitnum1; i++)
                {
                    MeleeUnit mU2 = new MeleeUnit(this,main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    mU2.Name = "M1";
                    mU2.UName = "Slug";
                    mU2.XPos = x;
                    mU2.YPos = y;
                    mU2.Fraction = t2;
                    arrPb[x, y] = mU2;
                }
                //This populates the map with melee unit two, swordman
                for (int i = 0; i < T2mUnitnum2; i++)
                {
                    MeleeUnit mU2 = new MeleeUnit(this, main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    mU2.Name = "M2";
                    mU2.UName = "Swordsman";
                    mU2.Speed = 0;
                    mU2.Atk = 30;
                    mU2.XPos = x;
                    mU2.YPos = y;
                    mU2.Fraction = t2;
                    arrPb[x, y] = mU2;
                }
                // This populates the map with ranged Unit one, rifleman
                for (int i = 0; i < T2rUnitnum1; i++)
                {
                    RangedUnit rU2 = new RangedUnit(this,main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);

                    while (arrPb[x, y] == mU)
                    {
                        rnd = new Random(Guid.NewGuid().GetHashCode());
                        rnd2 = new Random(Guid.NewGuid().GetHashCode());
                        x = rnd.Next(0, 20);
                        y = rnd2.Next(0, 20);
                    }
                    rU2.Name = "rU1";
                    rU2.UName = "Riffleman";
                    rU2.XPos = x;
                    rU2.YPos = y;
                    rU2.Fraction = t2;
                    arrPb[x, y] = rU2;
                }
                //This populates the map with ranged Unit two, missile launcher
                for (int i = 0; i < T2rUnitnum2; i++)
                {
                    RangedUnit rU2 = new RangedUnit(this, main);
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);

                    while (arrPb[x, y] == mU)
                    {
                        rnd = new Random(Guid.NewGuid().GetHashCode());
                        rnd2 = new Random(Guid.NewGuid().GetHashCode());
                        x = rnd.Next(0, 20);
                        y = rnd2.Next(0, 20);
                    }
                    rU2.Name = "rU2";
                    rU2.UName = "Boom Boom Guy";
                    rU2.atk = 50;
                    rU2.AtkRange = 3;
                    rU2.XPos = x;
                    rU2.YPos = y;
                    rU2.Fraction = t2;
                    arrPb[x, y] = rU2;
                }
                // This populates the map with Resource Buildings
                for (int i = 0; i < 1; i++)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    RB.Name = "RB";
                    RB.XPos = x;
                    RB.YPos = y;
                    RB.Fraction = " 1";
                    RB.Name = "RB";
                    arrPb[x, y] = RB;
                }
                // This populates the map with Factory Buildings
                for (int i = 0; i < 1; i++)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Random rnd2 = new Random(Guid.NewGuid().GetHashCode());
                    int x = rnd.Next(0, 20);
                    int y = rnd2.Next(0, 20);
                    FB.XPos = x;
                    FB.YPos = y;
                    FB.Fraction = " 1";
                    FB.Name = "FB";
                    arrPb[x, y] = FB;
                }
            }

            //place units
            public void placeUnits()
            {
                for (int i = 0; i < 20; i++)
                {
                    for (int l = 0; l < 20; l++)
                    {
                        if (arrPb[i, l].Name == "E")
                        {
                           
                            arrPb[i, l].SizeMode = PictureBoxSizeMode.Zoom;
                            arrPb[i, l].Anchor = AnchorStyles.Left;
                            //edit file path
                            arrPb[i, l].ImageLocation =
                                @"C:\Users\Irton\Pictures\poepics\bush.png";
                            arrPb[i, l].DoubleClick += new EventHandler(main.plane_Click);
                            gridM.Controls.Add(arrPb[i, l]);
                            

                        }
                        else if (arrPb[i, l].Name == "M1")
                        {
                            arrPb[i, l].Anchor = AnchorStyles.Left;
                            arrPb[i, l].SizeMode = PictureBoxSizeMode.Zoom;
                            arrPb[i, l].ImageLocation =
                                @"C:\Users\Irton\Pictures\poepics\MeleeUnit.png";
                            arrPb[i, l].DoubleClick += new EventHandler(main.melee_Click);
                            gridM.Controls.Add(arrPb[i, l]);
                         }
                        else if (arrPb[i, l].Name == "M2")
                        {
                            arrPb[i, l].Anchor = AnchorStyles.Left;
                            arrPb[i, l].SizeMode = PictureBoxSizeMode.Zoom;
                            arrPb[i, l].BorderStyle = BorderStyle.FixedSingle;
                            arrPb[i, l].ImageLocation =
                                @"C:\Users\Irton\Pictures\poepics\Swordsman.png";
                            arrPb[i, l].DoubleClick += new EventHandler(main.melee_Click);
                            gridM.Controls.Add(arrPb[i, l]);
                        }
                        else if (arrPb[i, l].Name == "rU1")
                        {
                            arrPb[i, l].Anchor = AnchorStyles.Left;
                            arrPb[i, l].SizeMode = PictureBoxSizeMode.Zoom;
                            arrPb[i, l].ImageLocation =
                                @"C:\Users\Irton\Pictures\poepics\RangedUnit.png";
                            arrPb[i, l].DoubleClick += new EventHandler(main.RangedUnit_Click);
                            gridM.Controls.Add(arrPb[i, l]);
                        }
                        else if (arrPb[i, l].Name == "rU2")
                        {
                            arrPb[i, l].Anchor = AnchorStyles.Left;
                            arrPb[i, l].SizeMode = PictureBoxSizeMode.Zoom;
                            arrPb[i, l].BorderStyle = BorderStyle.FixedSingle;
                            arrPb[i, l].ImageLocation =
                                @"C:\Users\Irton\Pictures\poepics\Boom.png";
                            arrPb[i, l].DoubleClick += new EventHandler(main.RangedUnit_Click);
                            gridM.Controls.Add(arrPb[i, l]);
                        }
                        else if (arrPb[i, l].Name == "FB")
                        {
                            arrPb[i, l].Anchor = AnchorStyles.Left;
                            arrPb[i, l].SizeMode = PictureBoxSizeMode.Zoom;
                            arrPb[i, l].ImageLocation =
                                @"C:\Users\Irton\Pictures\poepics\Factory.png";
                            arrPb[i, l].DoubleClick += new EventHandler(main.Factory_Click);
                            gridM.Controls.Add(arrPb[i, l]);
                        }
                        else if (arrPb[i, l].Name == "RB")
                        {
                            arrPb[i, l].Anchor = AnchorStyles.Left;
                            arrPb[i, l].SizeMode = PictureBoxSizeMode.Zoom;
                            arrPb[i, l].ImageLocation =
                                @"C:\Users\Irton\Pictures\poepics\Resource.png";
                            arrPb[i, l].DoubleClick += new EventHandler(main.Resource_Click);
                            gridM.Controls.Add(arrPb[i, l]);
                        }
                    }
                }
            }


            // This Method saves the units
            public void Save()
            {
                string FileName = @"D:\\RTS_Map.txt";
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, arrPb.ToString());
                stream.Close();
            }

            // This method read/ loads the units
            public void Read()
            {
                string FileName = @"D:\\RTS_Map.txt";
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                formatter.Deserialize(stream);
                stream.Close();
                placeUnits();

            }
        }

        // The GameEngine is the main class that links all the code together
        public class GameEngine
        {
            Form1 fm;
            public GameEngine(Form1 fm)
            {
                this.fm = fm;
                
            }
            // method to run the game
            public void Run()
            {
                for (int i = 0; i <= 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {

                        fm.rangedUnit.PosClosestUnit();
                        fm.meleeUnit.PosClosestUnit();
                    }
                }
                if (fm.meleeUnit.CanAttack())
                {
                    fm.meleeUnit.Attack();
                    fm.meleeUnit.Move();
                }
                else if (fm.rangedUnit.CanAttack())
                {
                    fm.rangedUnit.Attack();
                    fm.rangedUnit.Move();
                }
                else
                {
                    fm.meleeUnit.Move();
                    fm.rangedUnit.Move();
                }

            }

        }

        // Event handlers
        // This loads the game from start
        private void Form1_Load(object sender, EventArgs e)
        {
            Map map = new Map(gridM, this, RB1,eT);
            map.GenerateUnits();
            map.placeUnits();
        }

        // Click event for an empty plane that displays info on the tile
        private void plane_Click(object sender, EventArgs e)
        {
            unitStats.Text = sender.ToString();
        }

        // Click event for a melee unit that displays info on the tile
        private void melee_Click(object sender, EventArgs e)
        {
            unitStats.Text = sender.ToString();
        }

        // Click event for a ranged unit that displays info on the tile
        private void RangedUnit_Click(object sender, EventArgs e)
        {
            unitStats.Text = sender.ToString();
        }

        // Click event for a factory building that displays info on the tile
        private void Factory_Click(object sender, EventArgs e)
        { 
            unitStats.Text = sender.ToString();
        }

        // Click event for a resource building that displays info on the tile
        private void Resource_Click(object sender, EventArgs e)
        {
            unitStats.Text = sender.ToString();
        }

        // This button will enter the amounts of units a player wishes to start with
        private void enterInfo_Click(object sender, EventArgs e)
        {
            Map map1 = new Map(gridM, this, RB1, eT);
            if (mUnitCount1.Text == "" || mUnitCount2.Text == "" || mUnit2Count1.Text == "" || mUnit2Count1.Text == "" )
            {
                string str= "Info missing. Default troops will be used.";
                MessageBox.Show(str);
                map1.T1MUnitnum1 = 5;
                map1.T1MUnitnum2 = 5;
                map1.T1RUnitnum1 = 5;
                map1.T1RUnitnum2 = 5;
                map1.t1 = team1Name.Text;

                // team 2
                map1.T2MUnitnum1 = 5;
                map1.T2MUnitnum2 = 5;
                map1.T2RUnitnum1 = 5;
                map1.T2RUnitnum2 = 5;
                map1.t2 = team2Name.Text;
            }
            else
            {
                // sets team 1 units
                team1mU1 = Convert.ToInt32(mUnitCount1.Text);
                team1mU2 = Convert.ToInt32(mUnit2Count1.Text);
                team1rU1 = Convert.ToInt32(rUnitCount1.Text);
                team1rU2 = Convert.ToInt32(rUnit2Count1.Text);

                // sets team 2 units
                team2mU1 = Convert.ToInt32(mUnitCount2.Text);
                team2mU2 = Convert.ToInt32(mUnit2Count2.Text);
                team2rU1 = Convert.ToInt32(rUnitCount2.Text);
                team2rU2 = Convert.ToInt32(rUnit2Count2.Text);
                int t1total = team1mU1 + team1mU2 + team1rU1 + team1rU2;
                if (t1total == 20)
                {
                    // team 1
                    map1.T1MUnitnum1 = team1mU1;
                    map1.T1MUnitnum2 = team1mU2;
                    map1.T1RUnitnum1 = team1rU1;
                    map1.T1RUnitnum2 = team1rU2;
                    map1.t1 = team1Name.Text;

                    // team 2
                    map1.T2MUnitnum1 = team2mU1;
                    map1.T2MUnitnum2 = team2mU2;
                    map1.T2RUnitnum1 = team2rU1;
                    map1.T2RUnitnum2 = team2rU2;
                    map1.t2 = team2Name.Text;
                    gridM.Controls.Clear();
                    map1.GenerateUnits();
                    map1.placeUnits();
                }
                else
                    MessageBox.Show(" To many units! Make sure you have a total of 20 units per team");
            }
            
        }

        // This runs the timer 
        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;
            timerLabel.Text = tick.ToString();
            gm.Run();
            RB1.GenerateRes();
            RB2.GenerateRes();
            //FB1.Spawn();
            //FB2.Spawn();
            team1Cash.Text = RB1.ResCount.ToString();
            team2Cash.Text = RB2.ResCount.ToString();
        }

        // The starts the game and cause units to move
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (startBtn.Text == "Start")
            {
                timer1.Start();
                running = true;
                startBtn.Text = "Stop";
            }
            else
            {
                startBtn.Text = "Start";
                running = false;
                timer1.Stop();
            }
        }

        // This button save the game
        private void Savebtn_Click_1(object sender, EventArgs e)
        {

            MessageBox.Show("Save");
            if (running == false)
            {
                field.Save();
            }
        }

        // This button loads the game
        private void ReadData_Click_1(object sender, EventArgs e)
        {
            if (running == false)
            {
                field.Read();
            }
        }

        // This button Resets the game
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            tick = 0;
            timerLabel.Text = "0";
            gridM.Controls.Clear();
            Map map = new Map(gridM, this, RB1, eT);
            map.GenerateUnits();
            map.placeUnits();
            
        }

        // displays units info for in game info
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string str = "Melee Unit" + Environment.NewLine + "Name: Axel" + Environment.NewLine + "Team: 1" + Environment.NewLine + "Health: 80" + Environment.NewLine + "Attack: 30" + Environment.NewLine + "Speed: 1";
            unitStats.Text = str;
        }

        // displays units info for in game info
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            string str = "Melee Unit" + Environment.NewLine + "Name: Slug" + Environment.NewLine + "Team: 1" + Environment.NewLine + "Health: 100" + Environment.NewLine + "Attack: 20" + Environment.NewLine + "Speed: 2";
            unitStats.Text = str;
        }

        // displays units info for in game info
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string str = "Ranged Unit" + Environment.NewLine + "Name: Pablitos" + Environment.NewLine + "Team: 1" + Environment.NewLine + "Health: 50" + Environment.NewLine + "Attack: 10" + Environment.NewLine + "Speed: 1";
            unitStats.Text = str;
        }

        // displays units info for in game info
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string str = "Ranged Unit" + Environment.NewLine + "Name: Boom Boom Guy" + Environment.NewLine + "Team: 1" + Environment.NewLine + "Health: 50" + Environment.NewLine + "Attack: 50" + Environment.NewLine + "Speed: 1";
            unitStats.Text = str;
        }

        // displays units info for in game info
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            string str = "Melee Unit" + Environment.NewLine + "Name: Slug" + Environment.NewLine + "Team: 1" + Environment.NewLine + "Health: 100" + Environment.NewLine + "Attack: 20" + Environment.NewLine + "Speed: 2";
            unitStats.Text = str;
        }




        // These are eventhandlers that are not working or do nothing
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void grid_Paint(object sender, PaintEventArgs e)
        {

        }
        private void ReadData_Click(object sender, EventArgs e)
        {

        }
        private void SaveData_Click(object sender, EventArgs e)
        {

        }
        private void lblCash2_Click(object sender, EventArgs e)
        {
        }
        private void team1cash_Click(object sender, EventArgs e)
        {
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            
        }
    }
}
