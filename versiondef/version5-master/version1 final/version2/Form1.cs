using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        List<Point> posiciones = new List<Point>();
        string[] vectorconectados = new string[40]; // Vector para guardar los conectados
        string[] vectorjugadores = new string[4];
        int[] vectorposiciones = new int[40];
        
        int turno = 0;
        int turno2 = 0;
        int contadordeturno = 0;
        int numerojugadores;
        int posPerdedor;
        int[] posJugadores = new int[4];
        Random rand = new Random();
        string nombreficharojo;
        string nombrefichaverde;
        Socket server;
        Thread atender;
        int[] puntosFicha1 = new int[4];
        //Posiciones iniciales, y seguros donde no te pueden matar
        int[] posicioncasilla = new int[] { 130, 131, 132, 133 };
        int[] posicioncasillainicial = new int[] { -130, -81, -99, -116 };
        int[] posicioncasillasegura = new int[] { 0, 7, 12, 17, 24, 29, 34, 41, 46, 51, 58, 63, 130, 131, 132, 133 };
        
        int puntosFicha1Rojo, puntosFicha1Verde, puntosFicha1Amarillo, puntosFicha1Azul;

        delegate void DelegadoParaFinalizar();
        delegate void DelegadoParaGanar(string msg);
        delegate void DelegadoParaEscribir2(string sms);
        delegate void DelegadoParaFicha(int[] pos);
        delegate void DelegadoParaMostrarLabel(string[] rojo);
        delegate void DelegadoParaMostrar();
        delegate void DelegadoParaEscribir(string[] text);//Lo invoco y le envio un delegado, creo un delegado(objeto) y le asigno la funcion que tiene que hacer y al thread que invoco le envio el delegado
        public Form1()
        {
            InitializeComponent();
        }
       
        private void EscribirGanador(string msg)
        {
            GanadorLbl.Text = msg;
        }
        private void EscribeBox2(string mensajito)
        {
            textBox2.Text = mensajito;
        }
        private void EscribeBox3(string mensajito)
        {
            InvitadortextBox3.Text = mensajito;
        }

        private void EscribeConectados(string[] mensaje2)//escribe en el datagridview
        {
            label4.Visible = true;
            dataGridView1.Visible = true;
            Jugarbutton.Visible = true;
            dataGridView1.Rows.Clear();//limpiamos para que no se nos vuelva a repeir la lista en el data
            
            for (int i = 0; i < (mensaje2.Length - 1); i++)
            {
                dataGridView1.Rows.Add(mensaje2[i + 1]);
                vectorconectados[i] = mensaje2[i + 1];
            }      
        }
        private void NoVisibleData()
        {
            dataGridView1.Visible = false;
            label4.Visible = false;
        }

        private void NoVisibleLbl()
        {
            label3.Visible = false;
            labcolor.Visible = false;
            numeroLbl.Visible = false;
            label5.Visible = false;
            turnoLbl.Visible = false;
            iniciarPartida.Visible = false;
            //numeroinsert.Visible = false;
        }

        private void NoVisibleTxt()
        {
            Fecha.Visible = false;
            color_dame.Visible = false;
            textBox2.Visible = false;
            InvitadortextBox3.Visible = false;
        }

        private void NoVisibleBoton()
        {
            Ganador.Visible = false;
            Edad.Visible = false;
            Tantoporciento.Visible = false;
            button4.Visible = false;
            Jugarbutton.Visible = false;
            FinalizarPartida.Visible = false;
        }

        private void NoVisiblePanel() 
        
        { 
           tablero.Visible = false;
           panel1.Visible = false;
           panel2.Visible = false;  
        }

        private void VisiblelblCase1()
        {
            label3.Visible = true;
            labcolor.Visible = true;
        
        }
        private void VisibleTxtCase1()
        {
            Fecha.Visible = true;
            color_dame.Visible = true;

        }
        private void VisibleBotonCase1()
        {
            Ganador.Visible = true;
            Edad.Visible = true;
            Tantoporciento.Visible = true;
        }
        private void VisibleTxt()
        {
            Fecha.Visible = true;
            color_dame.Visible = true;
            textBox2.Visible = true;
            InvitadortextBox3.Visible = true;

        }

        private void VisibleBoton()
        {
            Ganador.Visible = true;
            Edad.Visible = true;
            Tantoporciento.Visible = true;
            button4.Visible = true;
            Jugarbutton.Visible = true;


        }

        private void VisiblePanel()
        {
            tablero.Visible = true;
        
        }
        private void VisiblePanelIf()
        {
            panel2.Visible = true;
            panel1.Visible = true;
            tablero.Visible = true;
            textBox2.Visible = true;
            Aceptarbtn.Visible = false;
            Rechazarbtn.Visible = false;

        }
        private void VisibleCase7()
        {
           
            panel1.Visible = true;
            InvitadortextBox3.Visible = true;

        }
        private void NoVisiblePanel1()
        {
            panel1.Visible = false;

        }
        private void CrearChatVisible()
        {
            chatlabel.Visible = true;
            Chatlbl.Visible = true;
            Chattxt.Visible = true;
            Chatbutton.Visible = true;
        }
        private void CrearChatNoVisible()
        {
            chatlabel.Visible = false;
            Chatlbl.Visible = false;
            Chattxt.Visible = false;
            Chatbutton.Visible = false;
        }

        private void VisibleParaJugar()
        {
            button4.Visible = true;
            numeroLbl.Visible = true;
        }
        private void EscribeChat(string chat)
        {
            Chatlbl.Text = chat;
        }

        private void EscribeLabelturno(string chat)
        {
            turnoLbl.Text = chat;
        }

        private void FinPartida()
        {
            button4.Visible = false;
            numeroLbl.Visible = false;
            button3.Visible = true;
            numeroLbl.Visible = true;
            Jugarbutton.Visible = true;
            darseBAJA.Visible = true;
            REGISTER.Visible = true;
            LOGIN.Visible = true;
            FinalizarPartida.Visible = false;
            nombreRojo.Text = "";
            nombreVerde.Text = "";
            nombreAzul.Text = "";
            nombreAmarillo.Text = "";
        }

        private void InicioPartida()
        {
            button3.Visible = false;
            numeroLbl.Visible = false;
            Jugarbutton.Visible = false;
            darseBAJA.Visible = false;
            REGISTER.Visible = false;
            LOGIN.Visible = false;
            FinalizarPartida.Visible = true;
        }

        private void MostraFicha(int[] pos)
        {
            
          

            //roja calculo de su posicion en el tablero
            if ((pos[0] > 62) && (pos[0] < 104))
            {
                pos[0] = pos[0] + 5;
                if (pos[0] > 73)
                {
                    DelegadoParaGanar d10 = new DelegadoParaGanar(EscribirGanador);
                    label3.Invoke(d10, new Object[] { nombreRojo.Text });
                    pos[0] = 96;
                   
                    puntosFicha1Rojo = pos[0];           

                }

            }
            puntosFicha1Rojo = pos[0];
            puntosFicha1[0] = pos[0];


            //verde calculo de su posicion en el tablero
            if ((pos[1] > 66) && (pos[1] < 129))
            {
                pos[1] = pos[1] - 67;
                
                if (pos[1] > 45)
                {
                    
                    pos[1] = pos[1] + 27;
                    
                    if (pos[1] > 80)
                    {
                         
                         DelegadoParaGanar d10 = new DelegadoParaGanar(EscribirGanador);
                         label3.Invoke(d10, new Object[] { nombreVerde.Text});
                         pos[1] = 96;
                                             
                    }
                }
            }
            puntosFicha1Verde = pos[1];
            puntosFicha1[1] = pos[1];

            // azul calculo de su posicion en el tablero
            if ((pos[2] > 66) && (pos[2] < 129))
            {
                pos[2] = pos[2] - 67;
               
                if (pos[2] > 29)
                {
                    pos[2] = pos[2] + 51;
                  
                    if (pos[2] > 87)
                    {
                        DelegadoParaGanar d10 = new DelegadoParaGanar(EscribirGanador);
                        label3.Invoke(d10, new Object[] {  nombreAzul.Text });
                        pos[2] = 96;
                       
                    }
                }
            }
            puntosFicha1Azul = pos[2];
            puntosFicha1[2] = pos[2];


            //amarillo calculo de su posicion en el tablero
            if ((pos[3] > 66) && (pos[3] < 129))
            {
                pos[3] = pos[3] - 67;
                if (pos[3] > 12)
                {
                    pos[3] = pos[3] + 76;

                    if (pos[3] > 95)
                    {
                        DelegadoParaGanar d10 = new DelegadoParaGanar(EscribirGanador);
                        label3.Invoke(d10, new Object[] { nombreAmarillo.Text });                        
                        pos[3] = 96;
                        
                    }
                }
            }
            puntosFicha1Amarillo = pos[3];
            puntosFicha1[3] = pos[3];
           
            ficharoja.Location = posiciones[pos[0]];
            fichaverde.Location = posiciones[pos[1]];
            fichaazul.Location = posiciones[pos[2]];
            fichaamarilla.Location = posiciones[pos[3]];

            
            if (posicioncasillasegura.Contains(pos[turno2]))
            {
                //MessageBox.Show("Estoy seguro");
            }
            else
            {
               // Calculo y envio en caso de ser matado para actualizar las posiciones en el servidor
                if ((pos[turno2] == pos[0]) && (turno2 != 0))
                {
                 
                    ficharoja.Location = posiciones[130];
                    pos[0] = 130;
                    if (vectorjugadores[0] == nombre.Text)
                    {
                        string mensaje = "18/" + 130 + "/" + pos[1] + "/" + pos[2] + "/" + pos[3] + "/"; //Enviamos a la ficha roja a casa
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }

                }
                else if ((pos[turno2] == pos[1] && (turno2 != 1)))
                {
                 
                    fichaverde.Location = posiciones[131];
                    pos[1] = 131;
                    if (vectorjugadores[0] == nombre.Text)
                    {
                        string mensaje = "18/" + pos[0] + "/" + 131 + "/" + pos[2] + "/" + pos[3] + "/"; //Enviamos a la ficha verde a casa
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                }
                else if ((pos[turno2] == pos[2] && (turno2 != 2)))
                {
                    
                    fichaazul.Location = posiciones[132];
                    pos[2] = 132;
                    if (vectorjugadores[0] == nombre.Text)
                    {
                        string mensaje = "18/" + pos[0] + "/" + pos[1] + "/" + 132 + "/" + pos[3] + "/"; //Enviamos a la ficha azul a casa
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                }
                else if ((pos[turno2] == pos[3] && (turno2 != 3)))
                {
                  
                    fichaamarilla.Location = posiciones[133];
                    pos[3] = 133;
                    if (vectorjugadores[0] == nombre.Text)
                    {
                        string mensaje = "18/" + pos[0] + "/" + pos[1] + "/" + pos[2] + "/" + 133 + "/"; //Enviamos a la ficha amarilla a casa
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                        server.Send(msg);
                    }
                }
            }
            
              posJugadores[0] = pos[0];
              posJugadores[1] = pos[1];
              posJugadores[2] = pos[2];
              posJugadores[3] = pos[3];         

        }
        
        
        
        private void MostrarLabelrojo(string[] jugadoresinvitados)
        {

            label5.Visible = true;
            turnoLbl.Visible = true;
            iniciarPartida.Visible = true;

            tablero.Controls.Add(ficharoja);
            tablero.Controls.Add(fichaverde);
            tablero.Controls.Add(fichaazul);
            tablero.Controls.Add(fichaamarilla);


            if (jugadoresinvitados.Length == 2)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[1];
            }
            else if (jugadoresinvitados.Length == 3)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[1];
                nombreVerde.Visible = true;
                nombreVerde.Text = jugadoresinvitados[2];

            }
            else if (jugadoresinvitados.Length == 4)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[1];
                nombreVerde.Visible = true;
                nombreVerde.Text = jugadoresinvitados[2];
                nombreAzul.Visible = true;
                nombreAzul.Text = jugadoresinvitados[3];

            }
            else if (jugadoresinvitados.Length == 5)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[1];
                nombreVerde.Visible = true;
                nombreVerde.Text = jugadoresinvitados[2];
                nombreAzul.Visible = true;
                nombreAzul.Text = jugadoresinvitados[3];
                nombreAmarillo.Visible = true;
                nombreAmarillo.Text = jugadoresinvitados[4];

            }
                       
        }
        // Muestra los nombres de los jugadores de las partidas
        private void MostrarLabels(string[] jugadoresinvitados)
        {


            tablero.Controls.Add(ficharoja);
            tablero.Controls.Add(fichaverde);
            tablero.Controls.Add(fichaazul);
            tablero.Controls.Add(fichaamarilla);


            if (numerojugadores == 1)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[0];
                nombreVerde.Text = "";
            }
            else if (numerojugadores == 2)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[0];
                nombreVerde.Visible = true;
                nombreVerde.Text = jugadoresinvitados[1];
                nombreAzul.Text = "";

            }
            else if (numerojugadores == 3)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[0];
                nombreVerde.Visible = true;
                nombreVerde.Text = jugadoresinvitados[1];
                nombreAzul.Visible = true;
                nombreAzul.Text = jugadoresinvitados[2];
                nombreAmarillo.Text = "";

            }
            else if (numerojugadores == 4)
            {
                nombreRojo.Visible = true;
                nombreRojo.Text = jugadoresinvitados[0];
                nombreVerde.Visible = true;
                nombreVerde.Text = jugadoresinvitados[1];
                nombreAzul.Visible = true;
                nombreAzul.Text = jugadoresinvitados[2];
                nombreAmarillo.Visible = true;
                nombreAmarillo.Text = jugadoresinvitados[3];

            }

            ficharoja.Location = posiciones[130];
            fichaverde.Location = posiciones[131];
            fichaazul.Location = posiciones[132];
            fichaamarilla.Location = posiciones[133];

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            DelegadoParaMostrar d1 = new DelegadoParaMostrar(NoVisibleLbl);
            label3.Invoke(d1, new Object[] {});
            labcolor.Invoke(d1, new Object[] {});
            numeroLbl.Invoke(d1, new Object[] { });

            DelegadoParaMostrar d2 = new DelegadoParaMostrar(NoVisibleTxt);
            Fecha.Invoke(d2, new Object[] { });
        
            DelegadoParaMostrar d3 = new DelegadoParaMostrar(NoVisibleBoton);
            Ganador.Invoke(d3, new Object[] { });
           
            DelegadoParaMostrar d4 = new DelegadoParaMostrar(NoVisiblePanel);
            tablero.Invoke(d4, new Object[] { });
            
            DelegadoParaMostrar d5 = new DelegadoParaMostrar(NoVisibleData);
            dataGridView1.Invoke(d5, new Object[] { });

            DelegadoParaMostrar d15 = new DelegadoParaMostrar(CrearChatNoVisible);
            chatlabel.Invoke(d15, new Object[] { });

            // posiciones del tablero
            tablero.Visible = true;
            posiciones.Add(new Point(183, 76));
            posiciones.Add(new Point(183, 95));
            posiciones.Add(new Point(188, 121));
            posiciones.Add(new Point(179, 135));
            posiciones.Add(new Point(162, 154));
            posiciones.Add(new Point(135, 158));
            posiciones.Add(new Point(110, 163));
            posiciones.Add(new Point(91, 163));
            posiciones.Add(new Point(70, 163));
            posiciones.Add(new Point(51, 163));
            posiciones.Add(new Point(28, 163));//10
            //10
            posiciones.Add(new Point(7, 163));
            posiciones.Add(new Point(7, 220));
            posiciones.Add(new Point(7, 281));
            posiciones.Add(new Point(27, 281));
            posiciones.Add(new Point(49, 281));
            posiciones.Add(new Point(69, 281));
            posiciones.Add(new Point(90, 281));
            posiciones.Add(new Point(111, 280));
            posiciones.Add(new Point(134, 284));
            posiciones.Add(new Point(155, 293)); //20
            //20
            posiciones.Add(new Point(169, 305));
            posiciones.Add(new Point(197, 315));
            posiciones.Add(new Point(208, 335));
            posiciones.Add(new Point(185, 357));
            posiciones.Add(new Point(183, 380));
            posiciones.Add(new Point(181, 405));
            posiciones.Add(new Point(181, 433));
            posiciones.Add(new Point(250, 435));
            posiciones.Add(new Point(321, 436));
            posiciones.Add(new Point(321, 421)); //30
            //30
            posiciones.Add(new Point(319, 405));
            posiciones.Add(new Point(318, 381));
            posiciones.Add(new Point(320, 368));
            posiciones.Add(new Point(296, 335));
            posiciones.Add(new Point(296, 315));
            posiciones.Add(new Point(328, 305));
            posiciones.Add(new Point(344, 293));
            posiciones.Add(new Point(364, 286));
            posiciones.Add(new Point(390, 284));
            posiciones.Add(new Point(410, 284));//40
            //40
            posiciones.Add(new Point(428, 285));
            posiciones.Add(new Point(451, 284));
            posiciones.Add(new Point(471, 284));
            posiciones.Add(new Point(494, 284));
            posiciones.Add(new Point(494, 220));
            posiciones.Add(new Point(494, 154));
            posiciones.Add(new Point(471, 154));
            posiciones.Add(new Point(451, 158));
            posiciones.Add(new Point(429, 163));
            posiciones.Add(new Point(408, 163)); //50
            //50
            posiciones.Add(new Point(380, 174));
            posiciones.Add(new Point(357, 172));
            posiciones.Add(new Point(340, 154));
            posiciones.Add(new Point(330, 135));
            posiciones.Add(new Point(319, 121));
            posiciones.Add(new Point(318, 95));
            posiciones.Add(new Point(318, 76));
            posiciones.Add(new Point(319, 55));
            posiciones.Add(new Point(320, 39));
            posiciones.Add(new Point(320, 20));//60
            //60
            posiciones.Add(new Point(320, 0));
            posiciones.Add(new Point(251, 0));
            posiciones.Add(new Point(181, 0));
            posiciones.Add(new Point(181, 20));
            posiciones.Add(new Point(181, 39));
            posiciones.Add(new Point(181, 55));//66
            //rojo 67
            posiciones.Add(new Point(250, 20));
            posiciones.Add(new Point(250, 39));
            posiciones.Add(new Point(253, 55));
            posiciones.Add(new Point(251, 76));
            posiciones.Add(new Point(251, 95));
            posiciones.Add(new Point(253, 112));
            posiciones.Add(new Point(253, 129));//73
            //verde 74
            posiciones.Add(new Point(471, 222));
            posiciones.Add(new Point(451, 222));
            posiciones.Add(new Point(430, 222));
            posiciones.Add(new Point(408, 222));
            posiciones.Add(new Point(387, 222));
            posiciones.Add(new Point(364, 222));
            posiciones.Add(new Point(344, 222));//80
            //azul 81
            posiciones.Add(new Point(249, 421));
            posiciones.Add(new Point(249, 405));
            posiciones.Add(new Point(249, 385));
            posiciones.Add(new Point(249, 365));
            posiciones.Add(new Point(249, 351));
            posiciones.Add(new Point(249, 335));
            posiciones.Add(new Point(249, 325));
            posiciones.Add(new Point(249, 301));//88
            //amarillo 89
            posiciones.Add(new Point(30, 224));
            posiciones.Add(new Point(50, 224));
            posiciones.Add(new Point(70, 224));
            posiciones.Add(new Point(90, 224));
            posiciones.Add(new Point(110, 224));
            posiciones.Add(new Point(132, 224));
            posiciones.Add(new Point(156, 224));//95
            //centro
            posiciones.Add(new Point(250, 221));//96
            posiciones.Add(new Point(250, 221));//97
            posiciones.Add(new Point(250, 221));//98
            posiciones.Add(new Point(250, 221));//99
            posiciones.Add(new Point(250, 221));//100
            posiciones.Add(new Point(250, 221));//101
            posiciones.Add(new Point(250, 221));//102
            posiciones.Add(new Point(250, 221));//103
            //modifico
            posiciones.Add(new Point(250, 221));//
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//103
            posiciones.Add(new Point(250, 221));//115
            posiciones.Add(new Point(250, 221));//115
            posiciones.Add(new Point(250, 221));//115
            posiciones.Add(new Point(250, 221));//115
            posiciones.Add(new Point(250, 221));//115
            posiciones.Add(new Point(250, 221));//115
            posiciones.Add(new Point(250, 221));//115
            posiciones.Add(new Point(250, 221));//122
            posiciones.Add(new Point(250, 221));//123
            posiciones.Add(new Point(250, 221));//124
            posiciones.Add(new Point(250, 221));//125
            posiciones.Add(new Point(250, 221));//126
            posiciones.Add(new Point(250, 221));//127
            posiciones.Add(new Point(250, 221));//128
            posiciones.Add(new Point(250, 221));//129
            ////esquina roja
            //posiciones.Add(new Point(67, 46));//104
            ////esquina verde
            //posiciones.Add(new Point(442, 46));//105
            ////esquina azul
            //posiciones.Add(new Point(448, 392));//106
            ////esquna amarilla
            //posiciones.Add(new Point(63, 388));//107

            //esquina roja
            posiciones.Add(new Point(67, 46));//130
            //esquina verde
            posiciones.Add(new Point(442, 46));//131
            //esquina azul
            posiciones.Add(new Point(448, 392));//132
            //esquna amarilla
            posiciones.Add(new Point(63, 388));//133


 
        }

        private void tablero_MouseMove(object sender, MouseEventArgs e)
        {
            GanadorLbl.Text = e.X.ToString() + "," + e.Y.ToString();
        }

        string acepta;
        int id;
        string NombreInvitador;
        int idposible;
        int activo;
        string nombreact;
        
        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string msg5 = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                //MessageBox.Show("mensaje recibido " + msg5 + nombre + "finmensaje");
                string[] trozos = msg5.Split('/');

               int codigo = Convert.ToInt32(trozos[0]);
              
                string mensaje = trozos[1];
                string mensajevuelta;
                string mensajevuelta2;
                string mensajecrear;
                string Nombreconv;

                switch (codigo)
                {

                    case 1:  // LogIn
                         DelegadoParaMostrar d6 = new DelegadoParaMostrar(VisiblelblCase1);
                         label3.Invoke(d6, new Object[] {});
                         DelegadoParaMostrar d7 = new DelegadoParaMostrar(VisibleTxtCase1);
                         Fecha.Invoke(d7, new Object[] {});
                         DelegadoParaMostrar d8 = new DelegadoParaMostrar(VisibleBotonCase1);
                         Ganador.Invoke(d8, new Object[] {});


                        MessageBox.Show("Estas dentro: " + mensaje);
                        break;
                    case 2:  // Edad media de los ganadores con ese color 

                        MessageBox.Show("Edad media: " + mensaje);
                        break;
                    case 3:  // Ganador en esa fecha 

                        MessageBox.Show("El ganador es: " + mensaje);
                        break;

                    case 4:  // Tanto por ciento de partidas ganadas

                        MessageBox.Show("Tanto por ciento de partidas ganadas con ese color es: " + mensaje);
                        break;

                    case 5: // listadeconectados
                        //MessageBox.Show("Se ha conectado lista: " + mensaje);
                          string[] partes = Encoding.ASCII.GetString(msg2).Split('/');
                          string mensajefinal = partes[1].Split('\0')[0];
                          string[] mensaje2 = mensajefinal.Split('|');
                          
                        DelegadoParaEscribir dele = new DelegadoParaEscribir(EscribeConectados);
                        dataGridView1.Invoke(dele, new Object[]{mensaje2});

                       
                        break;

                    case 6: //Muestra si se ha aceptado la peticion
                      
                        mensajevuelta = trozos[1].Split('\0')[0];
                        acepta = trozos[2].Split('\0')[0];
                       
                        DelegadoParaMostrar d9 = new DelegadoParaMostrar(VisiblePanelIf);
                        DelegadoParaEscribir2 d10 = new DelegadoParaEscribir2(EscribeBox2);
                        

                        if ((mensajevuelta == "Acepta") && (acepta != nombre.Text))
                        {
                           
                            panel2.Invoke(d9, new Object[] {});

                            textBox2.Invoke(d10, new Object[] { acepta + " acepta la inviacion" });
                        }
                        else if ((mensajevuelta == "Rechazar") && (acepta != nombre.Text))
                        {
                            
                            panel2.Invoke(d9, new Object[] { });
                            textBox2.Invoke(d10, new Object[] { acepta + " rechaza la inviacion" });
                        }
                        else
                        {
                            MessageBox.Show("Yo no me puedo autoinvitar");
                        }
                        break;

                    case 7: //Invitar

                        mensajevuelta = trozos[1].Split('\0')[0];
                        NombreInvitador = trozos[2].Split('\0')[0];
                      

                        DelegadoParaMostrar d11 = new DelegadoParaMostrar(VisibleCase7);
                        DelegadoParaEscribir2 d12 = new DelegadoParaEscribir2(EscribeBox3);
                        DelegadoParaMostrar d13 = new DelegadoParaMostrar(NoVisiblePanel1);
                        DelegadoParaMostrar d17 = new DelegadoParaMostrar(VisiblePanel);


                        if (mensajevuelta == "Invitado")
                        {
                            tablero.Invoke(d17, new Object[] { });
                            panel1.Invoke(d11, new Object[] { });
                            InvitadortextBox3.Invoke(d12, new Object[] {NombreInvitador});
                       
                        }
                        else
                        {
                            tablero.Invoke(d17, new Object[] { });
                            panel1.Invoke(d13, new Object[] { });
                         
                        }
                        break;

                    case 8: // Recibo una frase chat
                        mensajevuelta = trozos[1].Split('\0')[0];
                        Nombreconv = trozos[2].Split('\0')[0];
                        DelegadoParaEscribir2 d16 = new DelegadoParaEscribir2(EscribeChat);
                        Chatlbl.Invoke(d16, new Object[] {Chatlbl.Text + Nombreconv + ":" + mensajevuelta + "\n"});
                        
                        break;

                    case 9: //Mostramos CHAT

                       
                        
                        id = idposible;
                        mensajevuelta = trozos[1].Split('\0')[0];
                        mensajevuelta2 = trozos[2].Split('\0')[0];
                        string[] mensajeinvitados = mensajevuelta2.Split('|');
                                    
                        if (mensajevuelta == "crear")
                        {                               
                            

                            DelegadoParaMostrar d14 = new DelegadoParaMostrar(CrearChatVisible);
                            chatlabel.Invoke(d14, new Object[] { });

                            DelegadoParaMostrarLabel d18 = new DelegadoParaMostrarLabel(MostrarLabelrojo);
                            nombreRojo.Invoke(d18, new Object[] { mensajeinvitados });                           
                               

                        }

                        else
                        {
                            MessageBox.Show("No se ha podido enviar nah :V");
                        }

                        

                        break;

                    case 10: //Enviamos posibe IDP

                        idposible = Convert.ToInt32(trozos[1].Split('\0')[0]);

                        break;

                    case 11: //Iniciar Partida

                        id = idposible;
                        mensajevuelta = trozos[1].Split('\0')[0];
                        string[] mensajejugadores = trozos[1].Split('|');
                        numerojugadores = (mensajejugadores.Length -1);
                        
                        for (int i = 0; i < (mensajejugadores.Length -1); i++)
                        {
                            vectorjugadores[i] = mensajejugadores[i+1];
                           
                        }

                        vectorposiciones[0] = 130;
                        vectorposiciones[1] = 131;
                        vectorposiciones[2] = 132;
                        vectorposiciones[3] = 133;

                       // Se prepara el inicio de la Partida
                        DelegadoParaMostrarLabel d100 = new DelegadoParaMostrarLabel(MostrarLabels);
                        nombreRojo.Invoke(d100, new Object[] { vectorjugadores});
                       
                        DelegadoParaMostrar d102 = new DelegadoParaMostrar (InicioPartida);
                        nombreRojo.Invoke(d102, new Object[] {});

                        DelegadoParaMostrar jug2 = new DelegadoParaMostrar(VisibleParaJugar);
                        button4.Invoke(jug2, new Object[] { });
                        numeroLbl.Invoke(jug2, new Object[] { });
                        turno = 0;
                        DelegadoParaEscribir2 jug1 = new DelegadoParaEscribir2(EscribeLabelturno);
                        turnoLbl.Invoke(jug1, new Object[] { vectorjugadores[turno] });
                       

                        break;

                    case 12: // Recibe los turnos y las posiciones de las fichas con el dado

                       
                        turno = Convert.ToInt32(trozos[1]); 
                        DelegadoParaEscribir2 jug12 = new DelegadoParaEscribir2(EscribeLabelturno);
                        turnoLbl.Invoke(jug12, new Object[] { vectorjugadores[turno] });
                       
                        turno2 = Convert.ToInt32(trozos[2]); 
                        mensajevuelta = trozos[3].Split('\0')[0];
                        string[] mensajeposiciones = trozos[3].Split('|');
                       
                        for (int i = 0; i < (mensajeposiciones.Length); i++)
                        {
                           
                            vectorposiciones[i] = Convert.ToInt32(mensajeposiciones[i]);
                          
                        }


                        DelegadoParaFicha d21 = new DelegadoParaFicha(MostraFicha);
                         ficharoja.Invoke(d21, new Object[] { vectorposiciones});                         

                        break;
                    case 13:  // El registro satisfactorio o no y el Borrado de usuarios

                        MessageBox.Show(nombre.Text + ": " + mensaje);
                        break;

                    case 14: //Se recibe el turno y las posiciones de los jugadores al iniciar partida


                        mensajevuelta = trozos[2].Split('\0')[0];
                        string[] mensajeposiciones2 = trozos[2].Split('|');
                        
                        for (int i = 0; i < (mensajeposiciones2.Length); i++)
                        {

                            vectorposiciones[i] = Convert.ToInt32(mensajeposiciones2[i]);
                         
                        }

                   
                       
                        DelegadoParaFicha d23 = new DelegadoParaFicha(MostraFicha);
                        ficharoja.Invoke(d23, new Object[] { vectorposiciones });

                        break;

                    case 15: //FIN PARTIDA

                        MessageBox.Show(nombre.Text + ": " + mensaje);
                        DelegadoParaFinalizar d101 = new DelegadoParaFinalizar(FinPartida);
                        nombreRojo.Invoke(d101, new Object[] {});
                        

                        break;
                }



            }
        }

            private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.106"); //147.83.117.22 ip de shiva //192.168.56.102
            IPEndPoint ipep = new IPEndPoint(direc, 9093); // 50084 //9101


            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

            //pongo en marcha el thread que atenderá los mensajes del servidor
            ThreadStart ts = delegate { AtenderServidor(); };
            atender = new Thread(ts);
            atender.Start();
        }

            private void button2_Click(object sender, EventArgs e)
        {
            if (LOGIN.Checked)
            {
                //Enviamos nombre y contraseña 
                if ((nombre.Text == "") || (contraseña.Text == ""))
                {
                    MessageBox.Show("Introduce Nombre o Contraseña");
                }
                else
                {
                    string mensaje = "1/" + nombre.Text + "/" + contraseña.Text;
                    // Enviamos al servidor el nombre tecleado y  la contraseña
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
            }
            else if (REGISTER.Checked)
            {
                //Se hace el registro del usuario 
                if ((nombre.Text == "") || (contraseña.Text == ""))
                {
                    MessageBox.Show("Introduce Nombre o Contraseña");
                }
                else
                {
                    string mensaje = "16/" + nombre.Text + "/" + contraseña.Text;
                    // Enviamos al servidor el nombre tecleado y  la contraseña
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
            }
            else if (darseBAJA.Checked)
            {
                //Se elimina el usuario
                if ((nombre.Text == "") || (contraseña.Text == ""))
                {
                    MessageBox.Show("Introduce Nombre o Contraseña");
                }
                else
                {
                    string mensaje = "17/" + nombre.Text + "/" + contraseña.Text;
                    // Enviamos al servidor el nombre tecleado y  la contraseña
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
            }
            else if (Edad.Checked)
            {
               string mensaje = "2/" + color_dame.Text;
              // Enviamos al servidor el nombre tecleado
              byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
              server.Send(msg);

            }
            else if (Ganador.Checked)
            {
                string mensaje = "3/" + Fecha.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

            }
            else if (Tantoporciento.Checked)
            {
                
                  string mensaje = "4/" + color_dame.Text;
                
                  // Enviamos al servidor el nombre tecleado
                  byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                  server.Send(msg);

                     
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/"+ nombre.Text + "/";
        
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
       
            atender.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();


        }
    
        private void Jugarbutton_Click(object sender, EventArgs e) //boton para invitar 
        {
            if (vectorconectados.Length == 0)
            {
                MessageBox.Show("Debe esperar a que se conecte al menos un usuario");
                return;
            }

            int invitados = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if (invitados > 0)
            {

                string mensaje = "7/" + nombre.Text + "/" + invitados + "/";
                for (int i = 0; i < invitados; i++)
                {
                    int row = dataGridView1.SelectedCells[i].RowIndex;
                    mensaje = mensaje + vectorconectados[row] + ",";

                }

                
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

            }
            else
                MessageBox.Show("Debe seleccionar al menos un jugador al que invitar");


        }
        // Se envia el inicio de Partida
        private void iniciarPartida_Click(object sender, EventArgs e)
        {
            string mensaje = "9/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            button3.Visible = false;


           

        }

        // Aceptar la invitacion
        private void Aceptarbtn_Click_1(object sender, EventArgs e)
        {
            id = idposible;
            string mensaje = "6/" + nombre.Text + "/" + NombreInvitador + "/1"; //Envio mi nombre y el nombre de quien me invita
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            panel1.Visible = false;

        }
        // Rechazar la invitacion
        private void Rechazarbtn_Click_1(object sender, EventArgs e)
        {
            string mensaje = "6/" + nombre.Text + "/" + NombreInvitador + "/0";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            server.Send(msg);
            panel1.Visible = false;
        }

        private void Chatbutton_Click(object sender, EventArgs e)  // Boton Enviar mensajes
        {

            string mensaje = "8/" +Chattxt.Text + "/" +nombre.Text + "/"; //Enviamos el mensaje

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            server.Send(msg);
        }

       

        private void Nombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void LOGIN_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label2_AutoSizeChanged(object sender, EventArgs e)
        {

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Labcolor_Click(object sender, EventArgs e)
        {

        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Contraseña_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void NumJugadores_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Conectados_Click(object sender, EventArgs e)
        {

        }
        //Dado para mandar la posicion de cada jugador, hace la casuistica del 5
        private void button4_Click(object sender, EventArgs e)
        {
           
            
            int num;
            int contador=0;
            num = rand.Next(1, 7);

            num = Convert.ToInt32(numeroinsert.Text);
            if (num == 5) contador = contador + 1;
           
            if (vectorjugadores[turno] != nombre.Text)
                MessageBox.Show("No es tu turno");
            else
            {
                // no es 5 pero estamos en casa, no se cuenta la posicion del dado
                if ((num != 5) && ( posicioncasilla.Contains(vectorposiciones[turno])))
                {
                    
                    numeroLbl.Text = num.ToString();

                    string mensaje = "10/" + 0 + "/" + turno + "/" + "0" + "/"; //Enviamos el mensaje
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                 
                   
                }
                    // es un 5 y estamos en casa, salimos al tablero a jugar
                else if ((num == 5) && (posicioncasilla.Contains(vectorposiciones[turno])))
                {
                    
                    numeroLbl.Text = num.ToString();
                    string mensaje = "10/" + posicioncasillainicial[turno] + "/" + turno + "/" + "1" + "/"; //Enviamos el mensaje
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
                // cualquier numero y no estamos en casa,cuenta la posicion
                else if ((!posicioncasilla.Contains(vectorposiciones[turno])))
                {
                   
                    numeroLbl.Text = num.ToString();
                    string mensaje = "10/" + numeroLbl.Text + "/" + turno + "/" + "1" + "/"; //Enviamos el mensaje
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);
                }
            }
           
                
        }

        private void Chatbutton4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        // Finaliza la partida y se calcula los puntos de cada jugador
        private void FinalizarPartida_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Se finalizó la partida");
            button3.Visible = true;
           
            Array.Sort(puntosFicha1);
           // Calculo de la posicion final para contar los puntos como posiciones recorridas
            if (posJugadores[0] == 130)
            {
                posJugadores[0] = posJugadores[0] - 130;
            }
            if (posJugadores[1] == 131)
            {
                posJugadores[1] = posJugadores[1] - 131;
            }
            else
            {
                posJugadores[1] = posJugadores[1] - 51;
            }

            if (posJugadores[2] == 132)
            {
                posJugadores[2] = posJugadores[2] - 132;
            }
            else
            {
                posJugadores[2] = posJugadores[2] - 34;
            }

            if (posJugadores[3] == 133)
            {
                posJugadores[3] = posJugadores[3] - 133;
            }
            else
            {
                posJugadores[3] = posJugadores[3] - 17;
            }
            if (numerojugadores == 2)
            {
                posJugadores[2] = 132;
                posJugadores[3] = 133;
            }
            if (numerojugadores == 3)
            {
                posJugadores[3] = 133;
            }
           
            string fechapartida = DateTime.Now.ToString("yyyy-MM-dd");
            //string fechapartida = DateTime.Now.ToString("yyyy-MM-dd h:mm");
            int posPerdedor = Array.IndexOf(posJugadores, posJugadores.Min());         
         
            string mensaje = "15/" + GanadorLbl.Text + "/" + fechapartida + "/" + vectorjugadores[posPerdedor] + "/" + posJugadores[posPerdedor] + "/" + posJugadores[0] + "/" + posJugadores[1] + "/" + posJugadores[2] + "/" + posJugadores[3] + "/"; //Enviamos el mensaje
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);


        }

        

       
    }
}
