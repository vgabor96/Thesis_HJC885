using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm_ForMovement : MonoBehaviour
{
	
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
			route = new List<Vector3>();
		}

		public Member(List<Vector3> towns)
		{
			this.ID = allID++;
			route = new List<Vector3>();
			foreach (Vector3 item in towns)
			{
				route.Add(item);
			}
		}
		public List<Vector3> route;
		public double fitness;

		public int ID { get => id; set => id = value; }
	}

	int populationlimit = 1000; //200
	double achivefitness = 400;
	double bestfitness = double.MaxValue;
	//double previousfitness = double.MaxValue;
	double mutationrate = 8; //30
	int iteration = 1000000; //10000
	int actiteration = 0;
	int N = 30; //30
	List<Member> M = new List<Member>();

	public string Start(List<Vector3> S)
	{
		List<Member> P = INITIALIZEPOPULATION(S);
		EVALUATION(P);
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
			EVALUATION(P);
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

	private string GPM(Member pbest)
	{
		string result = "";
		for (int i = 0; i < pbest.route.Count - 2; i++)
		{
			result += pbest.route[i].x + " " + pbest.route[i].y + " " + pbest.route[i + 1].x + " " + pbest.route[i + 1].y;
		}
		//log.log_file.Close();
		return result;
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
		for (int i = 0; i < c.route.Count; i++)
		{


			if (Random.Range(0, 100) <= this.mutationrate)
			{

				//for (int i = 0; i < Utils.Utils.r.Next(1,3); i++) // (1,4)
				//{
				Swap(c.route, i, Random.Range(0, c.route.Count));
				//}
			}
		}


		return c;
	}

	private Member CROSSOVER(List<Member> p1_pk)
	{

		Member c = new Member();

		//int i = 0;
		//while (i < p1_pk[0].route.Count)
		//{
		//	int decision = Utils.Utils.r.Next(0, 3);
		//	if (decision <= 1)
		//	{
		//		if (!isTownalreadyvisited(c, p1_pk[0].route[i]))
		//		{
		//			c.route.Add(p1_pk[0].route[i]);

		//		}
		//		else if (!isTownalreadyvisited(c, p1_pk[1].route[i]))
		//		{
		//			c.route.Add(p1_pk[1].route[i]);

		//		}
		//		else
		//		{
		//			foreach (Town item in p1_pk[0].route)
		//			{
		//				if (!isTownalreadyvisited(c, item))
		//				{
		//					c.route.Add(item);
		//					break;
		//				}
		//			}
		//		}
		//	}
		//	else
		//	{
		//		if (!isTownalreadyvisited(c, p1_pk[1].route[i]))
		//		{
		//			c.route.Add(p1_pk[1].route[i]);

		//		}
		//		else if (!isTownalreadyvisited(c, p1_pk[0].route[i]))
		//		{
		//			c.route.Add(p1_pk[0].route[i]);

		//		}
		//		else
		//		{
		//			foreach (Town item in p1_pk[1].route)
		//			{
		//				if (!isTownalreadyvisited(c, item))
		//				{
		//					c.route.Add(item);
		//					break;
		//				}
		//			}
		//		}
		//	}
		//	i++;
		//}


		int rand1 =Random.Range(4, p1_pk[0].route.Count);
		int rand2 = rand1 - 2;

		Vector3[] croute = new Vector3[p1_pk[0].route.Count];

		List<Vector3> selectedtowns = new List<Vector3>();

		if (p1_pk[0].fitness < p1_pk[1].fitness)
		{


			for (int i = 0; i < 2; i++)
			{
				selectedtowns.Add(p1_pk[0].route[rand2 + i]);

			}
			for (int i = 0; i < 2; i++)
			{
				croute[rand2 + i] = selectedtowns[i];
			}

			for (int i = 0; i < p1_pk[1].route.Count; i++)
			{
				for (int j = 0; j < p1_pk[1].route.Count; j++)
				{
					if (croute[i] == null && !croute.Contains(p1_pk[1].route[j]))
					{
						croute[i] = p1_pk[1].route[j];

					}
				}
			}


		}
		else
		{

			for (int i = 0; i < 2; i++)
			{
				selectedtowns.Add(p1_pk[1].route[rand2 + i]);

			}
			for (int i = 0; i < 2; i++)
			{
				croute[rand2 + i] = selectedtowns[i];
			}

			for (int i = 0; i < p1_pk[0].route.Count; i++)
			{
				for (int j = 0; j < p1_pk[0].route.Count; j++)
				{
					if (croute[i] == null && !croute.Contains(p1_pk[0].route[j]))
					{
						croute[i] = p1_pk[0].route[j];

					}
				}
			}
		}


		for (int i = 0; i < croute.Length; i++)
		{
			c.route.Add(croute[i]);
		}
		return c;
	}

	private List<Member> SELECTION(List<Member> m)
	{

		List<Member> matingpool = new List<Member>();

		if (Random.Range(0,2) == 0)
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
		return /*iteration < actiteration ||*/ bestfitness <= achivefitness;
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

	private void EVALUATION(List<Member> p)
	{
		foreach (Member item in p)
		{
			item.fitness = objective(item.route);
		}
	}

	private List<Member> INITIALIZEPOPULATION(List<Vector3> towns)
	{
		movements = towns;
		List<Member> S = generateFixnumberPermutations(towns);


		List<Member> P = new List<Member>();
		int i = 0;
		Member m = new Member();
		while (i < populationlimit)
		{
			m = S[Random.Range(0, S.Count)];
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
			if (item.route.SequenceEqual(m.route))
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

	private List<Member> generateFixnumberPermutations(List<Vector3> towns)
	{

		int act = 0;
		bool isdistinct = false;

		List<Member> S = new List<Member>();
		while (act < populationlimit)
		{
			List<Vector3> randomroute = generateRandomroute(towns);
			isdistinct = true;
			foreach (Member item in S)
			{
				if (Enumerable.SequenceEqual(randomroute, item.route))
				{
					isdistinct = false;
					break;
				}
			}
			if (isdistinct)
			{
				S.Add(new Member(randomroute));
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
	private List<Vector3> generateRandomroute(List<Vector3> towns)
	{
		List<Vector3> randomroute = new List<Vector3>();
		foreach (Vector3 item in towns)
		{
			randomroute.Add(item);
		}

		Shuffle(randomroute);

		return randomroute;


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
			if (item.route.Distinct().ToList().Count < movements.Count)
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


