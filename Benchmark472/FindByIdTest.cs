using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmark472
{
	[MemoryDiagnoser]
	public class FindByIdTest
	{
		private ContactDto[] _contacts;
		private ClientDto[] _clients;

		[Params(10, 100, 1000)]
		public int ParentSize;

		//[Params(10, 100)]
		//public int ChildSize;

		[GlobalSetup]
		public void Setup()
		{
			_contacts = Enumerable.Range(1, ParentSize)
				.Select(i => new ContactDto { Id = i, Data = i.ToString() })
				.ToArray();

			_contacts.Shuffle();

			_clients = Enumerable.Range(1, ParentSize)
				.Select(i => new ClientDto { Id = i, ContactId = _contacts[i % ParentSize].Id })
				.ToArray();

			_clients.Shuffle();
		}

		[Benchmark]
		public ClientDto[] BuildClientsWithSearch()
		{
			foreach (var client in _clients)
			{
				client.ContactData = _contacts.FirstOrDefault(c => c.Id == client.ContactId)?.Data;
			}

			return _clients;
		}

		[Benchmark(Baseline = true)]
		public ClientDto[] BuildClientsWithMap()
		{
			var contactsLookup = _contacts.ToLookup(c => c.Id);

			foreach (var client in _clients)
			{
				client.ContactData = contactsLookup[client.ContactId].FirstOrDefault()?.Data;
			}

			return _clients;
		}

		public static void Run()
		{
			BenchmarkRunner.Run<FindByIdTest>();
		}
	}

	public class ContactDto
	{
		public int Id { get; set; }
		public string Data { get; set; }
	}

	public class ClientDto
	{
		public int Id { get; set; }
		public int ContactId { get; set; }
		public string ContactData { get; set; }
	}

	public static class EnumerableExtensions
	{
		public static void Shuffle<T>(this T[] array)
		{
			var random = new Random();
			var n = array.Length;

			while (n > 1)
			{
				var k = random.Next(n--);
				(array[n], array[k]) = (array[k], array[n]);
			}
		}
	}
}