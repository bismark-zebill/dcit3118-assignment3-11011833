// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthCareSystem
{
    //a. Generic Repository for managing entities
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return items;
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // b. Patient class
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }
    }

    // c. Prescription class
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }
    }

    // g. HealthSystemAp to manage patients and prescriptions
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new Repository<Patient>();
        private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

        //seed repository with sample data
        public void SeedData()
        {
            // Add patients
            _patientRepo.Add(new Patient(1, "Kwame Nkrumah", 28, "Male"));
            _patientRepo.Add(new Patient(2, "Kofi Baboni", 23, "Male"));
            _patientRepo.Add(new Patient(3, "Ella Akosuah", 80, "Female"));

            // add prescriptions (linked by PatientId)
            _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Paracetamol", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Metformin", DateTime.Now.AddDays(-3)));

        }

        // build dictionary mapping patient ID to their prescriptions
        public void BuildPrescriptionMap()
        {
            foreach (var prescription in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        // print all patients
        public void PrintAllPatients()
        {
            Console.WriteLine("=== Patient List ===");
            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender} ");
            }
            Console.WriteLine();
        }

        // f. get prescriptions by patient ID
        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.ContainsKey(patientId)
            ? _prescriptionMap[patientId]
            : new List<Prescription>();
        }

        // print prescriptions for a specific patient
        public void PrintPrescriptionsForPatient(int patientId)
        {
            var prescriptions = GetPrescriptionsByPatientId(patientId);
            if (prescriptions.Count == 0)
            {
                Console.WriteLine("No prescriptions found for this patient.");
                return;
            }

            Console.WriteLine($"=== Prescriptions for Patient {patientId} ===");
            foreach (var p in prescriptions)
            {
                Console.WriteLine($"Prescription ID: {p.Id}, Medication: {p.MedicationName}, Date: {p.DateIssued.ToShortDateString()}");
            }
            Console.WriteLine();
        }
    }

    // Main Program
    public class Program
    {
        public static void Main(string[] args)
        {
            HealthSystemApp app = new HealthSystemApp();

            app.SeedData();
            app.BuildPrescriptionMap();

            app.PrintAllPatients();

            Console.Write("Enter Patient ID to view prescriptions: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                app.PrintPrescriptionsForPatient(patientId);
            }
            else
            {
                Console.WriteLine("Invalid Patient ID.");
            }
        }
    }
}
