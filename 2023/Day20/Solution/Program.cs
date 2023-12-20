using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution;

public class ModuleSystem
{
    public Module Broadcaster { get; set; }
    public List<Module> Modules { get; set; } = new List<Module>();

    public Queue<Pulse> Pulses { get; } = new Queue<Pulse>();
    public List<Pulse> ProcessedPulses { get; } = new List<Pulse>();

    public ModuleSystem(Module broadcaster, List<Module> modules)
    {
        Broadcaster = broadcaster;
        Modules = modules;

        Modules.ForEach(m => m.System = this);
        Modules.ForEach(m => m.Initialize());
    }

    public bool ProcessPulse()
    {
        if (Pulses.TryDequeue(out var pulse))
        {
            var source = Modules.First(m => m.Name == pulse.Source.Name);
            var target = Modules.First(m => m.Name == pulse.Target.Name);
            target.Process(pulse);
            ProcessedPulses.Add(pulse);
            //Console.WriteLine(pulse + " (" + string.Join('\t', Pulses.Select(p => p.ToString())) + ")");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Initialize()
    {
        foreach (var module in Modules)
        {
            module.Initialize();
        }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var part1 = Part1(Parser.ParseInput(File.ReadAllText("input.txt")));
        var part2 = Part2(Parser.ParseInput(File.ReadAllText("input.txt")));

        Console.WriteLine($"Part 1: {part1} Part 2: {part2}");

        if (part1 != 731517480L || part2 != 244178746156661L)
        {
            throw new Exception();
        }
    }

    public static int Part1(ModuleSystem system)
    {
        //foreach (var module in system.Modules)
        //{
        //    Console.WriteLine($"\t{module.Name} [ label = \"{GetFullName(module)}\" ];");
        //}

        //Console.WriteLine();


        //foreach (var module in system.Modules)
        //{
        //    foreach (var output in module.Outputs)
        //    {
        //        Console.WriteLine($"\t{module.Name} -> {output.Name};");

        //    }
        //}

        for (var i = 0; i < 1000; i++)
        {
            system.Pulses.Enqueue(new Pulse(system.Broadcaster, system.Broadcaster, false));

            while (system.ProcessPulse())
            {
            }
        }

        var highPulses = system.ProcessedPulses.Count(p => p.Value);
        var lowPulses = system.ProcessedPulses.Count(p => !p.Value);

        return highPulses * lowPulses;
    }

    public static string GetFullName(Module module)
    {
        return module switch
        {
            FlipFlopModule m => "\\%" + m.Name,
            ConjunctionModule m => "\\&" + m.Name,
            _ => module.Name,
        };
    }

    public static long Part2(ModuleSystem system)
    {
        var target = system.Modules.Single(m => m.Name == "rx").Inputs.Single();
        var iterationCounts = system.Broadcaster.Outputs.Select(m => (long)FindIterationCount(system, m, target)).ToArray();

        var common = (long)iterationCounts[0]; ;

        foreach (var count in iterationCounts.Skip(1))
        {
            common = Arithmetic.LCM(common, count);
        }

        return common;
    }

    private static int FindIterationCount(ModuleSystem system, Module source, Module target)
    {
        var iteration = 0;
        system.Pulses.Clear();
        system.ProcessedPulses.Clear();

        while (true)
        {
            system.Pulses.Enqueue(new Pulse(system.Broadcaster, source, false));
            iteration++;
            while (system.ProcessPulse())
            {
                if (system.Pulses.TryPeek(out var nextPulse) && nextPulse.Target == target && nextPulse.Value)
                {
                    return iteration;
                }
            }
        }
    }
}

public record Pulse(Module Source, Module Target, bool Value)
{
    public override string ToString()
    {
        return $"{Source.Name} {(Value ? "-high" : "-low")} -> {Target.Name}";
    }
}

public class ModuleConnection
{
    public Module Module { get; set; }
    public bool Pulse { get; set; }
    public bool Memory { get; set; }

    public ModuleConnection(Module module)
    {
        Module = module;
    }
}

public abstract class Module
{
    public string Name { get; }
    public ModuleSystem System { get; set; } = default!;
    public List<Module> Inputs { get; set; } = new();
    public List<Module> Outputs { get; set; } = new();

    protected Module(string name)
    {
        Name = name;
    }


    public virtual void Initialize()
    {
    }

    public abstract void Process(Pulse pulse);

    protected void Broadcast(bool pulse)
    {
        foreach (var outputModule in Outputs)
        {
            System.Pulses.Enqueue(new Pulse(this, outputModule, pulse));
        }
    }
}

public class FlipFlopModule : Module
{
    private bool _state = false;

    public FlipFlopModule(string name) : base(name)
    {
    }

    public override void Process(Pulse pulse)
    {
        if (!pulse.Value)
        {
            _state = !_state;
            Broadcast(_state);
        }

    }
}

public class ConjunctionModule : Module
{
    private Dictionary<Module, bool> _inputMemory = new();

    public ConjunctionModule(string name) : base(name)
    {
    }

    public override void Initialize()
    {
        foreach (var input in Inputs)
        {
            _inputMemory[input] = false;
        }
    }

    public override void Process(Pulse pulse)
    {
        _inputMemory[pulse.Source] = pulse.Value;
        Broadcast(!_inputMemory.Values.All(m => m));
    }
}

public class BroadcastModule : Module
{
    public BroadcastModule(string name) : base(name)
    {
    }

    public override void Process(Pulse pulse)
    {
        Broadcast(pulse.Value);
    }
}

public class UntypedModule : Module
{
    public UntypedModule(string name) : base(name)
    {
    }

    public override void Process(Pulse pulse)
    {
    }
}