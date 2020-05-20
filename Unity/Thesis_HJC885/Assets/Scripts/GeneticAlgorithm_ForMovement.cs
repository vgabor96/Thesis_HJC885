using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneticAlgorithm_ForMovement : MonoBehaviour
{

	public Dictionary<Vector3, List<Vector3>> solvedmovements;
	private Vector3 bulletdest;
	public void Start()
	{
		bulletdest = new Vector3(0, 0, 0);
		solvedmovements = new Dictionary<Vector3, List<Vector3>>();
		solvedmovements = HandleTextFile.ReadSolutions();
	}

	List<Vector3> movements;
	public GeneticAlgorithm_ForMovement()
	{
		movements = new List<Vector3>();
	}
	public class Member
	{
		static int allID;
		private int id;
		public Member()
		{
			this.ID = allID++;
			movement = new List<Vector3>();
		}

		public Member(List<Vector3> towns)
		{
			this.ID = allID++;
			movement = new List<Vector3>();
			foreach (Vector3 item in towns)
			{
				movement.Add(item);
			}
		}
		public List<Vector3> movement;
		public double fitness;

		public int ID { get => id; set => id = value; }
	}

	int populationlimit = 5000; //200
	double achivefitness = 500;//99999;
	double bestfitness = double.MaxValue;
	//double previousfitness = double.MaxValue;
	double mutationrate = 10; //30
	int iteration = 500000; //10000
	int iterationneedstochangesignificantlymax = 10000;
	int iterationneedstochangesignificantly = 0;
	double needstochangevalue = 50;
	double previousbest = double.MaxValue;
	int actiteration = 0;
	int N = 20; //30
	int bodyparts = 3;
	 float from = -2f;
	 float to = 2f;
	List<Member> M = new List<Member>();

	private bool isVectorcontained(Vector3 bulletdest)
	{
		foreach (Vector3 item in solvedmovements.Keys)
		{
			if (AreVectorsEqual(item, bulletdest))
			{
				return true;
			}
		}
		return false;
	}

	private bool AreVectorsEqual(Vector3 v1, Vector3 v2)
	{
		double x1 = (double)v1.x;
		double y1 = (double)v1.y;
		double z1 = (double)v1.z;
		double x2 = (double)v2.x;
		double y2 = (double)v2.y;
		double z2 = (double)v2.z;
		return (x1 == x2) && (y1 == y2) && (z1 == z2);
	}
	public List<Vector3> StartFindingMovement(Vector3 bulletdest,Func<List<Vector3>, double> fitness)
	{
		//List<Vector3> v = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
		//fitness(v);
		if (isVectorcontained(bulletdest))
		{
			//fitness(solvedmovements[bulletdest]);
			return solvedmovements[bulletdest];
		}
		this.bulletdest = bulletdest;
		return Startsolve(fitness);



	}
	public List<Vector3> Startsolve(Func<List<Vector3>,double> fitness)
	{
		List<Member> P = INITIALIZEPOPULATION();
		EVALUATION(P, fitness);
		Member Pbest = Selectbest(P);
		while (fitness(Pbest.movement)>achivefitness)
		{
			previousbest = double.MaxValue;
			bestfitness = double.MaxValue;
			actiteration = 0;
			iterationneedstochangesignificantly = 0;


			P = INITIALIZEPOPULATION();
			P.OrderByDescending(x => x.fitness).ToList().RemoveRange(0, 1);
			P.Add(Pbest);
			EVALUATION(P, fitness);
			 Pbest = Selectbest(P);
			while (!STOPCONDITION())
			{
				List<Member> Pnew = SELECTPARENTS(P);
				while (Pnew.Count < N)
				{
					List<Member> p1_pk = SELECTION(M);
					Member c = CROSSOVER(p1_pk);
					c = MUTATE(c);
					Addchild(Pnew, c);
				}

				ModifyOldPopulation(P, Pnew);
				P = Pnew;
				noduplicatecheck(P);
				EVALUATION(P, fitness);
				Pbest = Selectbest(P);
				//appendFileLog(Pbest, iteration);
				appendConsoleLog(Pbest, iteration);

			}
			//fitness(Pbest.movement);
		}
		return GPM(Pbest);
	}

	private void ModifyOldPopulation(List<Member> p, List<Member> pnew)
	{
		p.OrderByDescending(x => x.fitness).ToList().RemoveRange(0, pnew.Count);
		foreach (Member item in pnew)
		{
			p.Add(item);
			noduplicatecheck(p);
		}

	}

	private List<Vector3> GPM(Member pbest)
	{

		//log.log_file.Close();
		Debug.Log("fitness:" + pbest.fitness);
		solvedmovements.Add(bulletdest, pbest.movement);
		HandleTextFile.WriteSolution(bulletdest,pbest.movement);
		return pbest.movement;
	}

	//private void appendFileLog(Member pbest, int iteration)
	//{
	//	if (pbest.fitness < bestfitness)
	//	{
	//		log.clearScreen();
	//		log.putIteration(actiteration);
	//		log.putFitness(pbest.fitness);
	//		foreach (Town var in owntowns)
	//		{
	//			log.putPoint(var.x, var.y, "Blue");
	//		}
	//		for (int i = 0; i < pbest.route.Count - 1; i++)
	//		{
	//			log.putArrow(pbest.route[i].x, pbest.route[i].y, pbest.route[i + 1].x, pbest.route[i + 1].y, "red");
	//		}

	//		bestfitness = pbest.fitness;
	//	}


	//}

	private void appendConsoleLog(Member pbest, int iteration)
	{

		//log.clearScreen();
		//previousbestfitness = pbest.fitness;
		//if (bestfitness > previousbestfitness)
		//{
		//	bestfitness = previousbestfitness;
		//}
		if (pbest.fitness < bestfitness)
		{
			//	Debug.Log("Iteration" + "\t" + actiteration);
			////Console.WriteLine("Fitness" + "\t" + pbest.fitness + System.Environment.NewLine);
			//Debug.Log("BEST Fitness:" + "\t" + bestfitness + System.Environment.NewLine);

			if (bestfitness + needstochangevalue < previousbest)
			{
				iterationneedstochangesignificantly = 0;

				previousbest = bestfitness;
			}
		
			
		
			
			bestfitness = pbest.fitness;
			
		}


	}

	private Member MUTATE(Member c)
	{
		for (int i = 0; i < c.movement.Count; i++)
		{
			if (UnityEngine.Random.Range(0, 100) <= this.mutationrate / 100)
			{
				switch (i)
				{
					case 0:
						c.movement[i] = RandomHeadMovement();
						break;
					case 1:
						c.movement[i] = RandomHeadrotation();
						break;
					case 2:
						c.movement[i] = RandomBodymovement();
						break;
					case 3:
						c.movement[i] = RandomBodyrotation();
						break;
					case 4:
						c.movement[i] = RandomLegMovement();
						break;
					case 5:
						c.movement[i] = RandomLegrotation();
						break;
					case 6:
						c.movement[i] = RandomFullBodyMovement();
						break;
					default:
						break;
				}

			}
		
		}
		

		return c;
	}

	private Member CROSSOVER(List<Member> p1_pk)
	{

		Member c = new Member();

		

		int rand1 = Random.Range(4, p1_pk[0].movement.Count);
		int rand2 = rand1 - 2;

		Vector3[] croute = new Vector3[p1_pk[0].movement.Count];

		foreach (Vector3 item in p1_pk[0].movement)
		{
			c.movement.Add(item);
		}
		
		for (int i = 0; i < c.movement.Count; i++)
		{
			int temp = Random.Range(0, 100) ;
			if (temp <= 50)
			{
				c.movement[i] = p1_pk[1].movement[i];
			}
		
		}


		return c;
	}

	private List<Member> SELECTION(List<Member> m)
	{

		List<Member> matingpool = new List<Member>();

		if (Random.Range(0,2) <= 1)
		{
			matingpool.Add(m.OrderBy(x => x.fitness).ToList()[0]);
			for (int i = 0; i < 1; i++)
			{
				int destiny = Random.Range(0, m.Count);
				matingpool.Add(m[destiny]);
			}
		}
		else
		{
			for (int i = 0; i < 2; i++)
			{
				int destiny = Random.Range(0, m.Count);
				matingpool.Add(m[destiny]);
			}

		}





		return matingpool;
	}

	private List<Member> SELECTPARENTS(List<Member> p)
	{
		List<Member> parents = new List<Member>();

		parents = p.OrderBy(x => x.fitness).Take(5).ToList();
		for (int i = 0; i < 5; i++)
		{
			parents.Add(p.OrderBy(x => x.fitness).ToList()[p.Count / 2 + i]);
		}
		//parents.Add(p.OrderByDescending(x => x.fitness).Take(1).FirstOrDefault());

		//for (int i = 0; i < 2; i++)
		//{
		//	parents.Add(p.OrderByDescending(x => x.fitness).ToList()[i];
		//}

		M = new List<Member>();
		foreach (Member item in parents)
		{
			M.Add(item);
		}

		return parents;
	}

	private bool STOPCONDITION()
	{
		actiteration++;

		if (actiteration % 1000 == 0)
		{

		}

		if (previousbest <= bestfitness + needstochangevalue)
		{
			iterationneedstochangesignificantly++;
		}

		//
		return iteration < actiteration || bestfitness <= achivefitness || iterationneedstochangesignificantly > iterationneedstochangesignificantlymax;
	}

	private Member Selectbest(List<Member> P)
	{
		Member Pact = P[0];
		for (int i = 1; i < P.Count; i++)
		{
			if (Pact.fitness > P[i].fitness)
			{
				Pact = P[i];
			}
		}


		return Pact;
	}

	private void EVALUATION(List<Member> p, Func<List<Vector3>,double> fitness)
	{
		foreach (Member item in p)
		{
			item.fitness = fitness(item.movement);
		}
	}

	private List<Member> INITIALIZEPOPULATION()
	{
		bestfitness = double.MaxValue;
		actiteration = 0;
		List<Member> S = generateRandomMembers();


		List<Member> P = new List<Member>();
		int i = 0;
		Member m = new Member() { movement=new List<Vector3>() {new Vector3(0,0,0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) } };
		Member m2 = new Member();

		if (Random.Range(0,2)==0)
		{
			m2.movement = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, -0.5f) };
		}
		else
		{
			m2.movement = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, +0.5f) };
		}

		
		P.Add(m);
		P.Add(m2);
		while (i < populationlimit-2)
		{
			m = S[UnityEngine.Random.Range(0, S.Count)];
			if (!this.isMemberalreadyinPop(P, m))
			{
				P.Add(m);
				i++;
			}

		}

		return P;
	}

	//HELPERS
	//private bool isTownalreadyvisited(Member m, Vector3 town)
	//{
	//	return m.route.Count(x => x.ID == town.ID) > 0;
	//}

	private bool isMemberalreadyinPop(List<Member> p, Member m)
	{
		foreach (Member item in p)
		{
			if (item.movement.SequenceEqual(m.movement))
			{
				return true;
			}
		}
		return false;
	}

	public  IList<T> Swap<T>(IList<T> list, int indexA, int indexB)
	{
		T tmp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = tmp;
		return list;
	}

	private List<Member> generateRandomMembers()
	{

		int act = 0;
		bool isdistinct = false;

		List<Member> S = new List<Member>();
		while (act < populationlimit)
		{
			List<Vector3> randommovement = generateRandomMovement(from,to);
			isdistinct = true;
			foreach (Member item in S)
			{
				if (Enumerable.SequenceEqual(randommovement, item.movement))
				{
					isdistinct = false;
					break;
				}
			}
			if (isdistinct)
			{
				S.Add(new Member(randommovement));
				act++;
			}


		}
		return S;
	}

	private List<Member> generateALlpossiblePermutations(List<Vector3> towns)
	{

		List<Member> S = new List<Member>();
		int limit = 100;
		int act = 0;


		foreach (var item in GetPermutations(towns, towns.Count - 1).ToList())
		{
			if (act > limit)
			{
				break;
			}
			S.Add(new Member(item as List<Vector3>));
			act++;

		}

		return S;
	}
	public  IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
	{
		if (length == 1) return list.Select(t => new T[] { t });
		return GetPermutations(list, length - 1)
			.SelectMany(t => list.Where(o => !t.Contains(o)),
				(t1, t2) => t1.Concat(new T[] { t2 }));
	}
	private List<Vector3> generateRandomMovement(float from, float to)
	{
		List<Vector3> movement = new List<Vector3>();
		//for (int i = 0; i < bodyparts*2; i++)
		//{
		//	Vector3 temp = new Vector3(Random.Range(from, to), Random.Range(from, to), Random.Range(from, to));
		
		//	movement.Add(temp);
		//}

		//Vector3 headmovement = new Vector3(Random.Range(-0.2f, 0.5f), Random.Range(-0.2f, 0.2f), Random.Range(-1f, 1f));
		//Vector3 headrotation = new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 30f));

		//Vector3 Bodymovement = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.1f, 0.1f), Random.Range(-0.2f, 0.2f));
		//Vector3 Bodyrotation = new Vector3(Random.Range(-15f, 15f), Random.Range(-90f, 90f), Random.Range(-90f, 90f));

		//Vector3 LegMovement = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.5f, 0.5f));
		//Vector3 Legrotation = new Vector3(Random.Range(-20f, 20f), Random.Range(-90f, 90f), Random.Range(-40f, 40f));


		movement.Add(RandomHeadMovement());
		movement.Add(RandomHeadrotation());
		movement.Add(RandomBodymovement());
		movement.Add(RandomBodyrotation());
		movement.Add(RandomLegMovement());
		movement.Add(RandomLegrotation());
		movement.Add(RandomFullBodyMovement());
		return movement;


	}

	private Vector3 RandomFullBodyMovement()
	{
		return new Vector3(0, 0, Random.Range(-1, 1f));
	}

	private Vector3 RandomHeadrotation()
	{ 
		return new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 30f));
	}
	private Vector3 RandomHeadMovement()
	{
		return new Vector3(Random.Range(-0.2f, 0.3f), Random.Range(-0.2f, 0.2f), Random.Range(-0.3f, 0.3f));
	}
	private Vector3 RandomBodyrotation()
	{
		return new Vector3(Random.Range(-15f, 15f), Random.Range(-90f, 90f), Random.Range(-90f, 90f));
	}
	private Vector3 RandomBodymovement()
	{
		return new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.1f, 0.1f), Random.Range(-0.2f, 0.2f));
	}
	private Vector3 RandomLegMovement()
	{
		return new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.5f, 0.5f));
	}
	private Vector3 RandomLegrotation()
	{
		return new Vector3(Random.Range(-20f, 20f), Random.Range(-90f, 90f), Random.Range(-40f, 40f));
	}

	private void Addchild(List<Member> population, Member newmember)
	{
		if (!isMemberalreadyinPop(population, newmember))
		{
			population.Add(newmember);
		}

	}

	private bool noduplicatecheck(List<Member> pop)
	{
		bool isduplicated = false;
		List<int> townids = new List<int>();
		for (int i = 0; i < movements.Count; i++)
		{
			townids.Add(i);
		}
		foreach (Member item in pop)
		{
			if (item.movement.Distinct().ToList().Count < movements.Count)
			{
				isduplicated = true;
				break;
			}

		}
		return isduplicated;

	}
	public void Shuffle<T>(IList<T> list)
	{ 
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = Random.Range(0,n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}


