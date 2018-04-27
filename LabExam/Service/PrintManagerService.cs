﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LabExam.Entity;
using LabExam.Exceptions;
using LabExam.Interfaces;

namespace LabExam.Service
{
    public class PrintManagerService : IPrinterManager
    {
        private IList<BasePrinter> printers = new List<BasePrinter>();

        private ILogger logger;

        public PrintManagerService(ILogger logger)
        {
            this.logger = logger;

            var epsonPrinter = new Printer("Canon", "123x");

            this.printers.Add(epsonPrinter);

            epsonPrinter.PrintEvent += PrintEventHandler;

            var canonPrinter = new Printer("Epson", "231");

            this.printers.Add(canonPrinter);

            canonPrinter.PrintEvent += PrintEventHandler;
        }

        public IList<BasePrinter> Printers => printers;

        public void PrintEventHandler(object sender, InfoPrintEventArgs info)
        {
            logger.Write($"Printer name: {info.Name} model: {info.Model} {info.Message} in {DateTime.Now}");
        }

        public void Add(string name, string model)
        {
            if (name == null)
                throw new ArgumentNullException($"Argument {nameof(name)} is null");

            if (model == null)
                throw new ArgumentNullException($"Argument {nameof(model)} is null");

            var resultFind = printers.FirstOrDefault(item => item.Name.Equals(name, StringComparison.CurrentCulture)
                        && item.Model.Equals(model, StringComparison.CurrentCulture));

            if (resultFind != null)
                throw new ExistPrinterException($"Printer with the same parametrs {nameof(name)} and {nameof(model)} already exist");

            var printer = new Printer(name, model);

            printer.PrintEvent += PrintEventHandler;

            printers.Add(printer);
        }

        public IList<BasePrinter> GetAll()
        {
            return this.Printers;
        }

        public void Print(BasePrinter printer, FileStream stream)
        {
            if (printer == null)
                throw new ArgumentNullException($"Argument {nameof(printer)} is null");

            if (stream == null)
                throw new ArgumentNullException($"Argument {nameof(printer)} is null");

            printer.Print(stream);
        }
    }
}