using System;

namespace ConsoleApp1
{
    class Particle
    {
        int dim;
        public double [] position;
        public double [] velocity;
        public double [] best_position;
        public double current_fitness;
        public double best_fitness;
        public Particle(int dim)
        {
            this.dim = dim;
            InitParticle();
            InitVelocity();
            //print_attribute();
        }


        public void InitParticle()
        {
            this.best_position = new double[this.dim];
            this.position = new double[this.dim];
            Random rand = new Random();
            for (int i = 0; i < dim; i++)
            {
                double val = rand.NextDouble() * 10;
                this.position[i] = val;
            }
            this.best_position = position;
        }

        public void InitVelocity()
        {
            this.velocity = new double[this.dim];
            for (int i = 0; i< dim; i++)
            {
                this.velocity[i] = 0;
            }
        }

        public void print_attribute()
        {
            Console.WriteLine("POSITION");
            for (int i = 0; i < this.dim; i++)
            {
                Console.WriteLine(this.position[i]);
            }
            Console.WriteLine("");
            Console.WriteLine("VELOCITY");
            for (int i = 0; i < this.dim; i++)
            {
                Console.WriteLine(this.velocity[i]);
            }
            Console.WriteLine("");
            Console.WriteLine("BEST POSITION");
            for (int i = 0; i < this.dim; i++)
            {
                Console.WriteLine(this.best_position[i]);
            }
            Console.WriteLine("");
            Console.WriteLine("CURRENT FITNESS");
            Console.WriteLine(this.current_fitness);
            Console.WriteLine("-------------------------");
        }
    }

    class Objective_function
    {
        public double Pakwahyu(double[] x)
        {
            double fx = Math.Pow((x[0] - 10), 2) + Math.Pow((x[1] - 15), 2);
            return fx;
        }

        public double PakWahyu_2(double[] x)
        {
            double a = x[0];
            double b = x[1];
            double c = x[2];
            double d = x[3];
            double fix = (a * a) + (b * b) + (c * c) + d;
            double gab = Math.Abs(fix - 255);
            return gab;
        }
    }
    class swarm
    {
        Particle [] particles;
        Objective_function obj = new Objective_function();
        int nSwarm;
        int max_iter;
        double c1 = 1;
        double c2 = 1;
        double w = 0.7;
        int dim;
        public double[] Gbest_position;
        public double Gbest_value;

        public swarm(int nSwarm, int dim, int max_iter)
        {
            this.dim = dim;
            this.nSwarm = nSwarm;
            this.max_iter = max_iter;
            this.particles = new Particle[this.nSwarm];
            MainProgram();
            
            //rint_particle();
        }

        private void createPopulation()
        {
            for (int i=0;i< this.nSwarm; i++)
            {
                this.particles[i] = new Particle(this.dim);
            }
        }

        private void CalculateFitness()
        {
            for (int i = 0; i < this.nSwarm; i++)
            {

                //double fit = this.obj.Pakwahyu(this.particles[i].position);
                double fit = this.obj.PakWahyu_2(this.particles[i].position);
                this.particles[i].current_fitness = fit;
            }
        }
        public void MainProgram()
        {
            createPopulation();
            CalculateFitness();
            //init_pbest();
            find_Gbest();
            Calculate_VeloAndPosition();
            for (int i = 0; i < this.nSwarm; i++)
            {
                this.particles[i].best_fitness = this.particles[i].current_fitness;
            }
            

            for (int iter = 0; iter < this.max_iter; iter++)
            {
                CalculateFitness();
                find_Gbest();
                pick_pbest();
                Calculate_VeloAndPosition();
            }
        }

        public void pick_pbest()
        {
            for (int i = 0; i < this.nSwarm; i++)
            {
                if (this.particles[i].best_fitness > this.particles[i].current_fitness)
                {
                    //do 
                    this.particles[i].best_fitness = this.particles[i].current_fitness;
                    this.particles[i].best_position = this.particles[i].position;
                }
            }
        }

        public void init_pbest()
        {
            for (int i = 0; i < this.nSwarm; i++)
            {
                this.particles[i].best_position = this.particles[i].position;
            }
        }
        public void print_particle()
        {
            for (int i = 0; i < this.nSwarm; i++)
            {
                this.particles[i].print_attribute();
            }
        }

        public void Calculate_VeloAndPosition()
        {
            Random rand = new Random();
            for (int i = 0; i< this.nSwarm; i++)
            {
                double[] new_velocity = new double[this.dim];
                double[] temp_velo = new double [this.dim];
                double[] temp_veloPbest = new double[this.dim];
                double[] temp_veloGbest = new double[this.dim];
                for (int j=0;j<this.dim;j++)
                {
                    temp_velo[j] = this.w * this.particles[i].velocity[j];
                    temp_veloPbest[j] = this.c1 * rand.NextDouble() * (this.particles[i].best_position[j] - this.particles[i].position[j]);
                    temp_veloGbest[j] = this.c2 * rand.NextDouble() * (this.Gbest_position[j] - this.particles[i].position[j]);
                    new_velocity[j] = temp_velo[j] + temp_veloPbest[j] + temp_veloGbest[j];
                    this.particles[i].velocity[j] = new_velocity[j];
                    this.particles[i].position[j] = this.particles[i].position[j] + new_velocity[j];
                }
            }

        }
        public void find_Gbest()
        {
            double min = this.particles[0].current_fitness;
            int index_Gbest = 0;
            for (int i = 1; i < this.nSwarm; i++)
            {
                if (min > this.particles[i].current_fitness){
                    min = this.particles[i].current_fitness;
                    index_Gbest = i;

                }
            }
            this.Gbest_value = min;
            this.Gbest_position = this.particles[index_Gbest].position;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ////Particle Burung = new Particle(2);
            ///
            swarm PSO = new swarm(100, 4, 100);
            Console.Write("GBEST : ");
            Console.WriteLine(PSO.Gbest_value);
            Console.WriteLine("");
            Console.WriteLine(PSO.Gbest_position[0]);
            Console.WriteLine(PSO.Gbest_position[1]);
            Console.WriteLine(PSO.Gbest_position[2]);
            Console.WriteLine(PSO.Gbest_position[3]);
            //Console.WriteLine(PSO.Gbest_value);
        }
    }
}
