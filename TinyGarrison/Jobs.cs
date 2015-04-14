using System;
using System.Collections.Generic;
using Styx;

namespace TinyGarrison
{
	enum JobType
	{
		Done,
		Move,
		GarrisonCache,
		Garden,
		Mine,
		Profession
	};

	class Job
	{
		public Job(JobType type, WoWPoint location)
		{
			Type = type;
			Location = location;
		}

		public JobType Type { get; set; }
		public WoWPoint Location { get; set; }
	}

	class Jobs
	{
		private static List<Job> MyJobs = new List<Job>();
		private static int _currentJobIndex;

		public static void Initialize()
		{
			Helpers.Log("Initializing..");

			Tasks.GarrisonCache.Check();
			Tasks.Garden.Check();
			Tasks.Mine.Check();
			Tasks.Profession.Check();
			Add(JobType.Done);
		}

		public static void NextJob()
		{
			_currentJobIndex++;
		}

		public static Job CurrentJob
		{
			get
			{
				return MyJobs[_currentJobIndex];
			}
		}

		public static void Add(JobType type, WoWPoint location)
		{
			MyJobs.Add(new Job(type, location));
		}

		public static void Add(JobType type)
		{
			Add(type, new WoWPoint(0, 0, 0));
		}
	}
}
