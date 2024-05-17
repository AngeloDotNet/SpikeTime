﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace UGIdotNET.SpikeTime.SpecFlow.Test.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class CarrelloDelleBirreFeature : object, Xunit.IClassFixture<CarrelloDelleBirreFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CarrelloDelleBirre.feature"
#line hidden
        
        public CarrelloDelleBirreFeature(CarrelloDelleBirreFeature.FixtureData fixtureData, UGIdotNET_SpikeTime_SpecFlow_Test_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "CarrelloDelleBirre", "A short summary of the feature", ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public void TestInitialize()
        {
        }
        
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Quando aggiungo una birra al carrello deve aumentare il numero degli elementi nel" +
            " carrello")]
        [Xunit.TraitAttribute("FeatureTitle", "CarrelloDelleBirre")]
        [Xunit.TraitAttribute("Description", "Quando aggiungo una birra al carrello deve aumentare il numero degli elementi nel" +
            " carrello")]
        [Xunit.TraitAttribute("Category", "PreparaElencoBirre")]
        public void QuandoAggiungoUnaBirraAlCarrelloDeveAumentareIlNumeroDegliElementiNelCarrello()
        {
            string[] tagsOfScenario = new string[] {
                    "PreparaElencoBirre"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Quando aggiungo una birra al carrello deve aumentare il numero degli elementi nel" +
                    " carrello", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 6
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 7
 testRunner.When("Seleziono la birra rossa", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 8
 testRunner.Then("Il numero di elementi nel carrello deve aumentare", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Quando aggiungo di nuovo la stessa birra deve incrementare la quantità dell\'eleme" +
            "nto nel carrello")]
        [Xunit.TraitAttribute("FeatureTitle", "CarrelloDelleBirre")]
        [Xunit.TraitAttribute("Description", "Quando aggiungo di nuovo la stessa birra deve incrementare la quantità dell\'eleme" +
            "nto nel carrello")]
        [Xunit.TraitAttribute("Category", "PreparaElencoBirre")]
        public void QuandoAggiungoDiNuovoLaStessaBirraDeveIncrementareLaQuantitaDellelementoNelCarrello()
        {
            string[] tagsOfScenario = new string[] {
                    "PreparaElencoBirre"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Quando aggiungo di nuovo la stessa birra deve incrementare la quantità dell\'eleme" +
                    "nto nel carrello", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 11
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 12
 testRunner.Given("Un carrello con una birra bionda", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 13
 testRunner.When("Aggiungo di nuovo la stessa birra", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 14
 testRunner.Then("Deve incrementare la quantità dell\'elemento nel carrello", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                CarrelloDelleBirreFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                CarrelloDelleBirreFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion