using System.IO;
using Xunit;

namespace SpiceSharpParser.IntegrationTests.DotStatements
{
    public class IncludeTests : BaseTests
    {
        [Fact]
        public void ContentOfIncludeIsInsertedAtSpecificPosition()
        {
            string c1Path = Path.Combine(Directory.GetCurrentDirectory(), "c1");
            string c2Path = Path.Combine(Directory.GetCurrentDirectory(), "c2");

            File.WriteAllText(c1Path, "*c1\n");
            File.WriteAllText(c2Path, "*c2\n");

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Comment circuit",
                ".include c1",
                "*Comment",
                ".include c2",
                ".END");

            Assert.Equal(3, model.Comments.Count);
            Assert.Equal("*c1", model.Comments[0]);
            Assert.Equal("*Comment", model.Comments[1]);
            Assert.Equal("*c2", model.Comments[2]);
        }

        [Fact]
        public void SingleInclude()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "diodesx.mod");
            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModel(
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                $".include \"{modelFilePath}\"",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));

            Assert.Null(exception);
        }

        [Fact]
        public void When_IncludeParentDirectory_Expect_Reference()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "diodesx.mod");
            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModel(
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                $".include ../diodesx.mod",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));

            Assert.Null(exception);
        }

        [Fact]
        public void SingleIncludeSingleQuotes()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "diodesy.mod");
            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModel(
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                $".include '{modelFilePath}'",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));

            Assert.Null(exception);
        }

        [Fact]
        public void SingleIncludeInSubcircuit()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "diodesx.mod");
            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModel(
                "Include - Diode circuit",
                "V1 OUT 0 0",
                "X1 OUT diode",
                ".SUBCKT diode output",
                "D1 output 0 1N914",
                $".include \"{modelFilePath}\"",
                ".ENDS",
                ".DC V1 -1 1 10e-3",
                ".SAVE i(V1)",
                ".END");
            var export = RunDCSimulation(model, "i(V1)");

            // Get reference
            double[] references =
            {
               2.520684772022719e-09, 2.520665232097485e-09, 2.520645248083042e-09, 2.520624819979389e-09, 2.520603725741921e-09, 2.520582409459848e-09, 2.520560649088566e-09, 2.520538000538863e-09, 2.520515129944556e-09, 2.520491593216434e-09, 2.520467612399102e-09, 2.520442965447955e-09, 2.520417652362994e-09, 2.520391229055008e-09, 2.520364583702417e-09, 2.520336828126801e-09, 2.520307962328161e-09, 2.520278874484916e-09, 2.520248454374041e-09, 2.520216701995537e-09, 2.520184505527823e-09, 2.520150754747874e-09, 2.520115449655691e-09, 2.520079700474298e-09, 2.520041952891461e-09, 2.520002650996389e-09, 2.519962460922898e-09, 2.519919828358752e-09, 2.519875419437767e-09, 2.519829456204548e-09, 2.519781050480674e-09, 2.519730646355356e-09, 2.519677799739384e-09, 2.519621844498943e-09, 2.519563668812452e-09, 2.519502162456888e-09, 2.519437547476855e-09, 2.519369379783143e-09, 2.519297437331147e-09, 2.519221276031658e-09, 2.519140673840070e-09, 2.519055408711779e-09, 2.518964592468365e-09, 2.518868003065222e-09, 2.518765085390839e-09, 2.518655506378309e-09, 2.518538155804606e-09, 2.518412922647428e-09, 2.518278474639146e-09, 2.518133923601340e-09, 2.517978381355590e-09, 2.517810848701174e-09, 2.517629882348160e-09, 2.517434039006616e-09, 2.517221764364308e-09, 2.516991060019791e-09, 2.516739705527016e-09, 2.516465702484538e-09, 2.516165609200982e-09, 2.515836872163391e-09, 2.515475161501968e-09, 2.515076480413825e-09, 2.514635832895351e-09, 2.514147445786818e-09, 2.513604324683172e-09, 2.512998475978634e-09, 2.512320573799798e-09, 2.511559182849510e-09, 2.510700980451475e-09, 2.509729757349533e-09, 2.508625973618450e-09, 2.507366425597013e-09, 2.505921525841615e-09, 2.504256357838130e-09, 2.502326790221332e-09, 2.500077533884593e-09, 2.497439421933478e-09, 2.494324247148683e-09, 2.490618655759391e-09, 2.486175321170236e-09, 2.480800564974572e-09, 2.474236482363779e-09, 2.466134130241215e-09, 2.456014613905211e-09, 2.443208080293857e-09, 2.426758793916406e-09, 2.405272869765440e-09, 2.377086694149710e-09, 2.341755483969976e-09, 2.297702500486665e-09, 2.242774105321033e-09, 2.174284835509965e-09, 2.088886258411193e-09, 1.982402894618041e-09, 1.849628367134315e-09, 1.684070757845824e-09, 1.477634958835239e-09, 1.220227058285062e-09, 8.992606936875092e-10, 4.990415580774510e-10, -4.208324063460023e-23, -6.222658915921997e-10, -1.398183520351370e-09, -2.365693620165477e-09, -3.572105541915782e-09, -5.076410555804323e-09, -6.952166481388744e-09, -9.291094477115180e-09, -1.220756418174318e-08, -1.584418615752092e-08, -2.037878504834723e-08, -2.603309548487864e-08, -3.308360396747645e-08, -4.187506874586688e-08, -5.283737797290300e-08, -6.650657008444583e-08, -8.355104497148602e-08, -1.048042475026989e-07, -1.313054202034536e-07, -1.643504193848955e-07, -2.055550786805860e-07, -2.569342167357824e-07, -3.210001533471285e-07, -4.008855497006358e-07, -5.004965768495850e-07, -6.247039000539800e-07, -7.795808144028804e-07, -9.727001679671332e-07, -1.213504582209257e-06, -1.513768057126441e-06, -1.888171507258285e-06, -2.355020333966173e-06, -2.937139061076621e-06, -3.662986687191783e-06, -4.568047149322574e-06, -5.696562662471649e-06, -7.103694343424394e-06, -8.858215224893939e-06, -1.104586649613992e-05, -1.377353976839135e-05, -1.717448782878606e-05, -2.141481549700064e-05, -2.670156304629412e-05, -3.329276978536466e-05, -4.150999799246158e-05, -5.175391113643180e-05, -6.452363948283857e-05, -8.044083562419591e-05, -1.002795274224200e-04, -1.250031216609715e-04, -1.558102032684916e-04, -1.941911156088105e-04, -2.419976971548277e-04, -3.015289829039203e-04, -3.756361388815854e-04, -4.678503519079946e-04, -5.825377853404534e-04, -7.250859365310891e-04, -9.021256392514054e-04, -1.121792314356274e-03, -1.394028558000970e-03, -1.730927332259435e-03, -2.147110349238757e-03, -2.660129130047428e-03, -3.290866177790397e-03, -4.063900589218683e-03, -5.007786885759424e-03, -6.155179858715831e-03, -7.542725667060601e-03, -9.210636215301937e-03, -1.120187715360643e-02, -1.356093587232876e-02, -1.633219609264214e-02, -1.955802291413677e-02, -2.327673904312388e-02, -2.752072521737903e-02, -3.231488373640667e-02, -3.767565498752745e-02, -4.361068391378264e-02, -5.011912351695047e-02, -5.719246701131020e-02, -6.481574162201031e-02, -7.296888192813844e-02, -8.162812138207687e-02, -9.076728201979800e-02, -1.003588891563969e-01, -1.103750792457605e-01, -1.207882998568939e-01, -1.315718201008491e-01, -1.427000796200868e-01, -1.541489071231517e-01, -1.658956380366401e-01, -1.779191572005734e-01, -1.901998880621483e-01, -2.027197453741645e-01, -2.154620644191900e-01, -2.284115164341036e-01, -2.415540172223232e-01, -2.548766338536659e-01, -2.683674927728656e-01, -2.820156914701786e-01
            };

            Assert.True(EqualsWithTol(export, references));
        }

        [Fact]
        public void SingleIncludeInc()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "diodes2.mod");
            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModel(
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                $".inc \"{modelFilePath}\"",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));

            Assert.Null(exception);
        }

        [Fact]
        public void SingleIncludeWorkingDirectoryFile()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "diodes3.mod");
            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                ".include \"diodes3.mod\"",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));

            Assert.Null(exception);
        }

        [Fact]
        public void WithoutQuotes()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "diodes4.mod");
            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                ".include diodes4.mod",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));

            Assert.Null(exception);
        }

        [Fact]
        public void SingleIncludeSubDirectoryLinuxStyleFile()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "common", "diodes.mod");

            string subdirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "common");
            if (!Directory.Exists(subdirectoryPath))
            {
                Directory.CreateDirectory(subdirectoryPath);
            }

            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                ".include \"common/diodes.mod\"",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));
            Assert.Null(exception);
        }

        [Fact]
        public void SingleIncludeSubDirectoryLinuxStyleFileWithoutQuotes()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "common", "diodes.mod");

            string subdirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "common");
            if (!Directory.Exists(subdirectoryPath))
            {
                Directory.CreateDirectory(subdirectoryPath);
            }

            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                ".include common/diodes.mod",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));
            Assert.Null(exception);
        }

        [Fact]
        public void SingleIncludeSubDirectoryWindowsStyleFile()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "common", "diodes.mod");

            string subdirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "common");
            if (!Directory.Exists(subdirectoryPath))
            {
                Directory.CreateDirectory(subdirectoryPath);
            }

            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                ".include \"common\\diodes.mod\"",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));
            Assert.Null(exception);
        }

        [Fact]
        public void SingleIncludeSubDirectoryWindowsStyleFileWithoutQuotes()
        {
            string modelFileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "common", "diodes.mod");

            string subdirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "common");
            if (!Directory.Exists(subdirectoryPath))
            {
                Directory.CreateDirectory(subdirectoryPath);
            }

            File.WriteAllText(modelFilePath, modelFileContent);

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                ".include common\\diodes.mod",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));
            Assert.Null(exception);
        }

        [Fact]
        public void NestedIncludeSubDirectoryFile()
        {
            string model1N914FileContent = ".model 1N914 D(Is=2.52e-9 Rs=0.568 N=1.752 Cjo=4e-12 M=0.4 tt=20e-9)\n";
            string model1N914FilePath = Path.Combine(Directory.GetCurrentDirectory(), "common", "1N914.mod");
            string modelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "common", "diodes.mod");
            string modelFileContent = ".include \"1N914.mod\"\n";

            string subdirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "common");
            if (!Directory.Exists(subdirectoryPath))
            {
                Directory.CreateDirectory(subdirectoryPath);
            }

            File.WriteAllText(modelFilePath, modelFileContent);
            File.WriteAllText(model1N914FilePath, model1N914FileContent);

            var model = GetSpiceSharpModelWithWorkingDirectoryParameter(
                Directory.GetCurrentDirectory(),
                "Include - Diode circuit",
                "D1 OUT 0 1N914",
                "V1 OUT 0 0",
                ".DC V1 -1 1 10e-3",
                ".SAVE V(OUT)",
                ".NODESET V(OUT)={x+1}",
                ".param x = 13",
                $".include \"common\\diodes.mod\"",
                ".END");

            var exception = Record.Exception(() => RunDCSimulation(model, "V(OUT)"));
            Assert.Null(exception);
        }
    }
}