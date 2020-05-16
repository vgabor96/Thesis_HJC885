using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneticAlgorithm_ForMovement : MonoBehaviour
{

	public void Start()
	{
		
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

	int populationlimit = 100; //200
	double achivefitness = 400;
	double bestfitness = double.MaxValue;
	//double previousfitness = double.MaxValue;
	double mutationrate = 8; //30
	int iteration = 100; //10000
	int actiteration = 0;
	int N = 20; //30
	int bodyparts = 3;
	 float from = -5f;
	 float to = 5f;
	List<Member> M = new List<Member>();

	public List<Vector3> Startsolve(Func<List<Vector3>,double> fitness)
	{
		List<Member> P = INITIALIZEPOPULATION();
		EVALUATION(P,fitness);
		Member Pbest = Selectbest(P);
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
			EVALUATION(P,fitness);
			Pbest = Selectbest(P);
			//appendFileLog(Pbest, iteration);
			appendConsoleLog(Pbest, iteration);

		}
		return GPM(Pbest);
	}

	private void ModifyOldPopulation(List<Member> p, List<Member> pnew)
	{
		p.OrderBy(x => x.fitness).ToList().RemoveRange(0, pnew.Count);
		foreach (Member item in pnew)
		{
			p.Add(item);
			noduplicatecheck(p);
		}

	}

	private List<Vector3> GPM(Member pbest)
	{

		//log.log_file.Close();
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
		//if (pbest.fitness < bestfitness)
		//{
		Debug.Log("Iteration" + "\t" + actiteration);
		//Console.WriteLine("Fitness" + "\t" + pbest.fitness + System.Environment.NewLine);
		Debug.Log("BEST Fitness:" + "\t" + bestfitness + System.Environment.NewLine);
		//	bestfitness = pbest.fitness;
		//}


	}

	private Member MUTATE(Member c)
	{
		for (int i = 0; i < c.movement.Count; i++)
		{


			if (UnityEngine.Random.Range(0, 100) <= this.mutationrate/100)
			{

				//for (int i = 0; i < Utils.Utils.r.Next(1,3); i++) // (1,4)
				//{
				c.movement[i] = new Vector3(Random.Range(from,to), Random.Range(from, to), Random.Range(from, to));
				//}
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
			else if (temp <= 75)
			{
				c.movement[i] = new Vector3(Random.Range(from, to), Random.Range(from, to), Random.Range(from, to));
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
		return iteration < actiteration || bestfitness <= achivefitness;
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
	
		List<Member> S = generateRandomMembers();


		List<Member> P = new List<Member>();
		int i = 0;
		Member m = new Member();
		while (i < populationlimit)
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
		for (int i = 0; i < bodyparts*2; i++)
		{
			Vector3 temp = new Vector3(Random.Range(from, to), Random.Range(from, to), Random.Range(from, to));
		
			movement.Add(temp);
		}

		return movement;


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


