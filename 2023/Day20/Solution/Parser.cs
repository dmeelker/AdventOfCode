using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution;

public static class Parser
{
    public record ParsedModule(string Name, string Type, List<string> Outputs);

    public static ModuleSystem ParseInput(string input)
    {
        var parsedModules = input.ToLines().Select(ParseModule).ToDictionary(m => m.Name);
        var modules = parsedModules.Values.Select(CreateModule).ToDictionary(m => m.Name);

        var untypedModules = parsedModules.Values.SelectMany(m => m.Outputs).Where(o => !parsedModules.ContainsKey(o)).ToArray();

        foreach (var m in untypedModules)
        {
            modules.Add(m, new UntypedModule(m));
        }

        foreach (var parsedModule in parsedModules.Values)
        {
            var module = modules[parsedModule.Name];

            foreach (var output in parsedModule.Outputs)
            {
                var targetModule = modules[output];
                module.Outputs.Add(targetModule);
                targetModule.Inputs.Add(module);
            }
        }

        var broadcaster = modules.Values.First(m => m.Name == "broadcaster");

        return new(broadcaster, modules.Values.ToList());
    }

    private static Module CreateModule(ParsedModule module)
    {
        return module.Type switch
        {
            "%" => new FlipFlopModule(module.Name),
            "&" => new ConjunctionModule(module.Name),
            "" => new BroadcastModule(module.Name),
            _ => throw new Exception("Unknown module type")
        };
    }

    private static ParsedModule ParseModule(string line)
    {
        var parts = line.Split(" -> ");
        var name = parts[0];
        var type = "";

        if (!char.IsLetter(name[0]))
        {
            type = name[0].ToString();
            name = name.Substring(1);
        }

        var targets = parts[1].Split(", ");

        return new ParsedModule(name, type, targets.ToList());
    }
}
