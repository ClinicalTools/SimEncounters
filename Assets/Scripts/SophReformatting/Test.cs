﻿using ClinicalTools.SimEncounters.XmlSerialization;
using ClinicalTools.SimEncounters.SerializationFactories;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using ClinicalTools.ClinicalEncounters.SerializationFactories;
using System.Diagnostics;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Collections;

public class Test : MonoBehaviour
{

    NodeInfo LegacyEncounterDataInfo = NodeInfo.RootValue;
    EncounterDataFactory EncounterDataFactory => new ClinicalEncounterDataFactory();
    ImageDataFactory ImageDataFactory => new ClinicalImageDataFactory();
    NodeInfo NewEncounterDataInfo = new NodeInfo("new");


    // Start is called before the first frame update
    void Start()
    {
        TestCed();
        TestCei();
    }

    bool TestCed()
    {
        UnityEngine.Debug.Log($"<b>CED</b>");

        var dataStr = data2;
        var dataFactory = EncounterDataFactory;

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(dataStr.Trim());

        Stopwatch stopwatch = Stopwatch.StartNew();

        var deserializer = new XmlDeserializer(xmlDoc);
        KeyGenerator.ResetKeyGenerator(0);
        var encounterData = deserializer.GetValue(LegacyEncounterDataInfo, dataFactory);


        var time = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Deserializing legacy time: {time}ms");
        stopwatch.Restart();

        var xmlDoc2 = new XmlDocument();
        var serializer2 = new XmlSerializer(xmlDoc2);
        serializer2.AddValue(NewEncounterDataInfo, encounterData, dataFactory);

        time = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Serializing time: {time}ms");
        stopwatch.Restart();

        var deserializer2 = new XmlDeserializer(xmlDoc2);
        KeyGenerator.ResetKeyGenerator(0);
        var encounterData2 = deserializer2.GetValue(NewEncounterDataInfo, dataFactory);

        time = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Deserializing time: {time}ms");

        var xmlDoc3 = new XmlDocument();
        var serializer3 = new XmlSerializer(xmlDoc3);
        serializer3.AddValue(NewEncounterDataInfo, encounterData2, dataFactory);

        var equal = xmlDoc3.InnerXml == xmlDoc2.InnerXml;
        UnityEngine.Debug.LogWarning($"New format can be deserialized and reserialized with no changes: <b>{equal}</b>");
        UnityEngine.Debug.Log(xmlDoc3.InnerXml);


        //EncounterData = encounterData2;

        return equal;
    }
    bool TestCei()
    {
        UnityEngine.Debug.Log($"<b>CEI</b>");

        var dataStr = legacyImgData1;
        var dataFactory = ImageDataFactory;

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(dataStr.Trim());

        Stopwatch stopwatch = Stopwatch.StartNew();

        var deserializer = new XmlDeserializer(xmlDoc);
        KeyGenerator.ResetKeyGenerator(0);
        var encounterData = deserializer.GetValue(LegacyEncounterDataInfo, dataFactory);

        var time = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Deserializing legacy time: {time}ms");
        stopwatch.Restart();

        var xmlDoc2 = new XmlDocument();
        var serializer2 = new XmlSerializer(xmlDoc2);
        serializer2.AddValue(NewEncounterDataInfo, encounterData, dataFactory);

        time = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Serializing time: {time}ms");
        stopwatch.Restart();

        var deserializer2 = new XmlDeserializer(xmlDoc2);
        KeyGenerator.ResetKeyGenerator(0);
        var encounterData2 = deserializer2.GetValue(NewEncounterDataInfo, dataFactory);

        time = stopwatch.ElapsedMilliseconds;
        UnityEngine.Debug.Log($"Deserializing time: {time}ms");

        var xmlDoc3 = new XmlDocument();
        var serializer3 = new XmlSerializer(xmlDoc3);
        serializer3.AddValue(NewEncounterDataInfo, encounterData2, dataFactory);

        var equal = xmlDoc3.InnerXml == xmlDoc2.InnerXml;
        UnityEngine.Debug.LogWarning($"New format can be deserialized and reserialized with no changes: <b>{equal}</b>");
        UnityEngine.Debug.Log(xmlDoc3.InnerXml);

        //ImageData = encounterData;

        return equal;
    }

    string data2 = @"
<?xml version=""1.0"" encoding=""UTF-8""?>
<data>
  <new>
    <sections>
      <section id=""_PATIENT_INTROSection"">
        <name>_PATIENT_INTROSection</name>
        <tabs>
          <tab id=""Case Overview"">
            <type>TextboxTab</type>
            <name>Case+Overview</name>
            <panels>
              <panel id=""3e1d5e1ab4"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3eNew+Patient%3a+Chad+Wright%2c+age+34%3c%2fb%3e%0a%0a%3cb%3ePresenting+Problem%3a%3c%2fb%3e+Mr.+Wright+is+looking+for+a+new+provider+to+prescribe+pain+medication+for+an+old+knee+injury.%0a%0a%3cb%3eScenario%3a%3c%2fb%3e+Chad+Wright+has+chronic+knee+pain+for+which+he+has+been+taking+an+opioid+analgesic.+He+is+currently+out+of+medication+and+looking+for+a+new+provider.+Finding+providers+who+are+willing+to+continue+prescribing+opioids+for+him+has+been+a+struggle+because+of+their+concerns+he+might+become+addicted.+%0a%0aMr.+Wright+traces+his+pain+back+to+a+football+injury+in+college.+Now+he+is+entering+middle+age+and+is+the+father+of+two+young+boys.+He+has+been+through+many+different+forms+of+treatment%2c+including+surgery+in+his+twenties+and+opioids%2c+which+he+has+taken+for+many+years.+Despite+these+treatments%2c+his+pain+continues+to+be+a+problem+if+he+doesn%27t+take+opioids+every+day.+He+would+like+to+be+more+active+with+his+children+and+wants+to+appear+stronger+in+front+of+them.+The+pain+and+limitations+contribute+to+being+depressed%2c+which+is+being+treated+with+antidepressants.+%0a%0aMr.+Wright+presents+to+this+medical+practice+as+a+new+patient%2c+requesting+a+prescription+for+the+long-acting+opioids+that+he+has+been+taking.</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""d92c6b0051"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Instructor</value>
                          <value id=""charColor"">RGBA(0.569%2c+0.569%2c+0.569%2c+1.000)</value>
                          <value id=""dialogueText"">%5bAnswers+phone%5d+Hello.+Smitherton+Clinic.+How+may+I+help+you%3f</value>
                        </values>
                      </panel>
                      <panel id=""a72850c43a"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+need+an+appointment+to+get+some+more+pain+medication+for+my+knee.</value>
                        </values>
                      </panel>
                      <panel id=""7c08f9d99f"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Instructor</value>
                          <value id=""charColor"">RGBA(0.569%2c+0.569%2c+0.569%2c+1.000)</value>
                          <value id=""dialogueText"">Okay.+And+have+you+been+here+before%3f</value>
                        </values>
                      </panel>
                      <panel id=""9125debd89"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">No%2c+I%27m+a+new+patient.+</value>
                        </values>
                      </panel>
                      <panel id=""6794e3d4b0"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Instructor</value>
                          <value id=""charColor"">RGBA(0.569%2c+0.569%2c+0.569%2c+1.000)</value>
                          <value id=""dialogueText"">Okay.+I+have+an+appointment+available+next+Tuesday+at+10+am.</value>
                        </values>
                      </panel>
                      <panel id=""ed6e1daff6"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+can%27t+wait+that+long.+I%27ve+run+out+of+my+prescription.+Can+you+fit+me+in+sooner%3f</value>
                        </values>
                      </panel>
                      <panel id=""5de310e997"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Instructor</value>
                          <value id=""charColor"">RGBA(0.569%2c+0.569%2c+0.569%2c+1.000)</value>
                          <value id=""dialogueText"">Well%2c+we+could+fit+you+in+tomorrow+at+8+am.+</value>
                        </values>
                      </panel>
                      <panel id=""0f0a4c4b73"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Okay.+I%27ll+take+it.+Thanks%21</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
            </panels>
          </tab>
          <tab id=""Personal Info"">
            <type>Personal_InfoTab</type>
            <name>Personal+Info</name>
            <panels>
              <panel id=""9847fe5ac1"">
                <type>BasicDetailsPanel</type>
                <values>
                  <value id=""RecordValue"">094118</value>
                  <value id=""FirstNameValue"">Chad</value>
                  <value id=""LastNameValue"">Wright</value>
                  <value id=""Gender"">Male</value>
                  <value id=""MonthValue"">09</value>
                  <value id=""DayValue"">03</value>
                  <value id=""YearValue"">1983</value>
                  <value id=""AgeValue"">34</value>
                  <value id=""AllergiesValue"">Penicillin</value>
                  <value id=""Education"">Bachelor%27s+Degree</value>
                </values>
              </panel>
              <panel id=""4d06b04a3f"">
                <type>AdditionalPanel</type>
                <values>
                  <value id=""AdditionalInformationValue"">Limited+mobility+due+to+chronic+knee+pain.+Uses+a+cane+periodically.+</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Office Visit"">
            <type>Office_VisitTab</type>
            <name>Office+Visit</name>
            <panels>
              <panel id=""b04dcb3226"">
                <type>OfficeVisitPanel</type>
                <values>
                  <value id=""BPValue1"">130</value>
                  <value id=""BPValue2"">80</value>
                  <value id=""PulseValue"">85</value>
                  <value id=""TempValue"">98.4</value>
                  <value id=""RespValue"">16</value>
                  <value id=""HeightValue"">70</value>
                  <value id=""WeightValue"">192</value>
                  <value id=""BMIValue"">27.5+kg%2fm%c2%b2</value>
                  <value id=""ChiefComplaintValue"">I+need+a+prescription+for+oxycodone+for+my+knee+pain+since+my+last+doctor+stopped+prescribing+it.</value>
                  <value id=""PainValue"">6</value>
                  <value id=""HoPIValue"">Chronic+knee+pain+for+over+15+years+for+which+he+has+been+taking+an+extended-release+opioid.+History+of+depression%2c+which+has+been+fairly+well-managed+with+antidepressant+medication.+%0a%0aHis+pain+is+fairly+well-controlled+by+taking+opioids%2c+but+sometimes+increases+after+periods+of+activity.+He+was+taking+his+next+dose+a+little+early+whenever+his+pain+worsened+and%2c+as+a+result%2c+he+ran+out+of+his+medications+early.+After+requesting+the+medication+early+several+times%2c+his+previous+provider+refused+to+write+another+prescription.+</value>
                </values>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
      <section id=""_HistorySection"">
        <name>_HistorySection</name>
        <tabs>
          <tab id=""Hx Info"">
            <type>TextboxTab</type>
            <name>Hx+Info</name>
            <panels>
              <panel id=""7f52e74b58"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3e%3ccolor%3d%23FFBE45%3eBACKGROUND+INFORMATION+FOR+HISTORY%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cb%3ePain+History%3c%2fb%3e%0a%0aBecause+pain+is+a+subjective+experience%2c+tools+such+as+pain+rating+scales+can+be+helpful+in+turning+the+patient%27s+experience+into+something+measurable%2c+such+as+a+rating+on+a+10-point+scale.+Be+sure+to+also+explore+the+patient%27s+emotional+and+cognitive+components+of+pain%2c+for+example%2c+by+asking+how+the+pain+is+affecting+their+life.+%0a%0aWhen+interviewing+a+patient+about+acute+or+chronic+pain+history%2c+an+acronym+such+as+the+PQRSTU+acronym+can+help%3a%0a%0a%3cb%3e%3ccolor%3d%23008080FF%3eP%3a%3c%2fcolor%3e%3c%2fb%3e+What+provokes+the+pain%3f+What+palliation+have+they+tried%3f+What+is+the+past+history+of+similar+pain%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eQ%3a%3c%2fcolor%3e%3c%2fb%3e+What+is+the+quality+of+the+pain%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eR%3a%3c%2fcolor%3e%3c%2fb%3e+Does+the+pain+radiate%3f++What+region+of+the+body+is+involved%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eS%3a%3c%2fcolor%3e%3c%2fb%3e+How+severe+is+the+pain%3f+Using+a+scale+of+1+to+10+can+help.%0a%3cb%3e%3ccolor%3d%23008080FF%3eT%3a%3c%2fcolor%3e%3c%2fb%3e+Timing+Factors%3a+Onset+-+When+did+it+start%3f+When+does+it+occur+now%3f+Any+patterns%3f+Duration%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eU%3a%3c%2fcolor%3e%3c%2fb%3e+How+does+the+pain+affect+you%3f+Include+impact+on+psychosocial+and+mechanical+functioning.+%0a%0a%3cb%3eOpioid+Prescribing+Guidelines%3c%2fb%3e%0aCurrent+opioid+prescribing+guidelines+published+by+the+CDC+for+chronic+pain+treatment+include+the+following+recommendations%3a%0a1.+Use+other+non-opioid+and+non-pharmacological+treatments+first+if+possible%0a2.+Prescribe+opioids+only+for+moderate+to+severe+pain+that+cannot+be+managed+by+other+treatments.%0a3.+If+opioids+are+prescribed%2c+only+a+3-day+supply+is+sufficient+for+most+acute+pain.+Prescriptions+should+last+for+the+duration+of+the+most+severe+pain%2c+not+for+the+duration+of+the+pain.%0a(Dowell+et+al.%2c+2016)%0a%0a%3cb%3eRegarding+past+pain+medications+described+in+this+case%3a%3c%2fb%3e%0aThe+FDA+recommended+against+the+continued+use+of+propoxyphene+(Darvon)+in+2010+because+of+heart+toxicity+(FDA%2c+2010).</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""ROS"">
            <type>Review_Of_SystemsTab</type>
            <name>ROS</name>
            <panels>
              <panel id=""89ecb4e3b5"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">General%2c+constitutional</value>
                </values>
              </panel>
              <panel id=""47444c0550"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Eyes%2c+vision</value>
                </values>
              </panel>
              <panel id=""4b0f5be9dc"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Ears%2c+nose%2c+throat</value>
                </values>
              </panel>
              <panel id=""ee6e84b235"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Cardiovascular</value>
                </values>
              </panel>
              <panel id=""92912b592a"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Respiratory</value>
                </values>
              </panel>
              <panel id=""5d966dfa08"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""CollapseCheckBoxToggle"">True</value>
                  <value id=""PanelNameValue"">Gastrointestinal</value>
                  <value id=""DetailsValue"">Opioid-induced+constipation.+</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""b390105479"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I+see+you+have+some+constipation.+Is+that+still+a+problem%3f</value>
                        </values>
                      </panel>
                      <panel id=""4e3cf9e51c"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">It%27s+not+much+of+a+problem+anymore.+It%27s+only+if+I+don%27t+take+a+stool+softener+and+eat+enough+fiber.</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
              <panel id=""d7f6ffddd3"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Genitourinary</value>
                </values>
              </panel>
              <panel id=""228f7570c5"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""CollapseCheckBoxToggle"">True</value>
                  <value id=""PanelNameValue"">Musculoskeletal</value>
                  <value id=""DetailsValue"">Right+knee+pain</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""ea19ec6d7c"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I+see+you+rated+your+pain+as+a+6+out+of+10+in+your+intake+form.+How+badly+does+it+hurt+at+different+times+of+the+day%3f</value>
                        </values>
                      </panel>
                      <panel id=""4b68a3084d"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">It+depends+more+on+how+active+I+am%2c+which+makes+it+worse%2c+and+whether+or+not+I%27m+taking+an+opioid%2c+which+gets+the+pain+down+to+around+a+2+out+of+10.+</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
              <panel id=""c80f6bf212"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Integumentary+%26+breast</value>
                </values>
              </panel>
              <panel id=""0fa985f7d6"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""CollapseCheckBoxToggle"">True</value>
                  <value id=""PanelNameValue"">Neurological</value>
                  <value id=""DetailsValue"">Mild+depression%2c+takes+Cymbalta%c2%ae+(duloxetine)</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""a1c257ec0b"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">And+you%e2%80%99re+taking+duloxetine+for+depression%2c+I+see.+How+well+is+that+working%3f</value>
                        </values>
                      </panel>
                      <panel id=""bcf1189fb9"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Pretty+well.</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
              <panel id=""2e6a486c11"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Psychiatric</value>
                </values>
              </panel>
              <panel id=""59df693164"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Endocrine</value>
                </values>
              </panel>
              <panel id=""9b7beaa30d"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Hematologic+%2f+lymphatic</value>
                </values>
              </panel>
              <panel id=""231684045c"">
                <type>ReviewOfSystemsPanel</type>
                <values>
                  <value id=""PanelNameValue"">Allergic+%2f+immunologic</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Med Events"">
            <type>Medical_EventsTab</type>
            <name>Med+Events</name>
            <panels>
              <panel id=""33601c2069"">
                <type>MedicalEventEntryPanel</type>
                <values>
                  <value id=""ProblemEventValue"">Anterior+cruciate+ligament+(ACL)+surgically+repaired+right+knee</value>
                  <value id=""OneTimeToggle"">True</value>
                  <value id=""StartMonthValue"">09</value>
                  <value id=""StartDayValue"">03</value>
                  <value id=""StartYearValue"">2002</value>
                  <value id=""DetailsValue"">Sports-related+injury+surgically+repaired.+Physician+Alfred+Chen%2c+MD.+%0a+%e2%80%a2+Post-surgical+function+good.+Post-surgical+pain+treated+with+oxycodone+plus+acetaminophen.%0a+%e2%80%a2+Physical+therapy+for+4+weeks%2c+once+per+week.+%0a+%e2%80%a2+Post-surgical+neuropathic+pain+continued%2c+treated+with+chronic+opioid+therapy.+%0a+%e2%80%a2+Constipation%2c+opioid-induced%2c+managed+with+stool+softener+and+laxative.+</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""e1eafe64d5"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">It+sounds+like+you%27ve+been+dealing+with+your+knee+pain+for+a+while.+When+did+it+start%3f</value>
                        </values>
                      </panel>
                      <panel id=""3f1c8ae260"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I%27ve+had+pain+since+a+football+injury%2c+around+15+years+ago.</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
              <panel id=""c82a9c79df"">
                <type>MedicalEventEntryPanel</type>
                <values>
                  <value id=""ProblemEventValue"">Endoscopic+surgical+removal+of+scar+tissue%2c+right+knee</value>
                  <value id=""OneTimeToggle"">True</value>
                  <value id=""StartMonthValue"">03</value>
                  <value id=""StartDayValue"">07</value>
                  <value id=""StartYearValue"">2009</value>
                  <value id=""DetailsValue"">Mild+improvement+in+post-surgical+neuropathic+pain+(Dr.+Jared+Smith).+Physical+therapy+was+recommended+for+two+months%2c+but+the+patient+self-discontinued+PT+after+one+month.+</value>
                </values>
              </panel>
              <panel id=""3721af40c7"">
                <type>MedicalEventEntryPanel</type>
                <values>
                  <value id=""ProblemEventValue"">Cognitive+Behavioral+Therapy+(CBT)</value>
                  <value id=""PeriodOfTimeToggle"">True</value>
                  <value id=""StartMonthValue"">04</value>
                  <value id=""StartDayValue"">03</value>
                  <value id=""StartYearValue"">2009</value>
                  <value id=""EndMonthValue"">04</value>
                  <value id=""EndDayValue"">24</value>
                  <value id=""EndYearValue"">2012</value>
                  <value id=""DetailsValue"">For+help+in+coping+with+pain+(3+sessions).</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""524daf9f32"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Did+the+counseling+help+you+cope+with+the+pain+and+stress%3f</value>
                        </values>
                      </panel>
                      <panel id=""8bc026da90"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+went+a+few+times+and+learned+about+managing+stress+and+my+feelings+about+having+pain.+And+it+did+help.+Taking+an+antidepressant+helps%2c+too.++</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
              <panel id=""05cfcb38ac"">
                <type>MedicalEventEntryPanel</type>
                <values>
                  <value id=""ProblemEventValue"">Hyaluronic+acid+injection%2c+right+knee</value>
                  <value id=""OneTimeToggle"">True</value>
                  <value id=""StartMonthValue"">07</value>
                  <value id=""StartDayValue"">03</value>
                  <value id=""StartYearValue"">2012</value>
                  <value id=""DetailsValue"">Provided+mild+pain+relief+for+several+weeks.</value>
                </values>
              </panel>
              <panel id=""01f1ca1f78"">
                <type>MedicalEventEntryPanel</type>
                <values>
                  <value id=""ProblemEventValue"">Mild+depression</value>
                  <value id=""OngoingToggle"">True</value>
                  <value id=""StartMonthValue"">06</value>
                  <value id=""StartDayValue"">17</value>
                  <value id=""StartYearValue"">2003</value>
                  <value id=""DetailsValue"">Multiple+episodes+of+mild+depression+due+to+pain+and+loss+of+ability+to+be+active+and+play+sports.+Treated+with+antidepressant%2c+duloxetine.</value>
                </values>
              </panel>
              <panel id=""f3f121d83a"">
                <type>MedicalEventEntryPanel</type>
                <values>
                  <value id=""ProblemEventValue"">Cortisone+injections%2c+right+knee</value>
                  <value id=""PeriodOfTimeToggle"">True</value>
                  <value id=""StartMonthValue"">11</value>
                  <value id=""StartDayValue"">11</value>
                  <value id=""StartYearValue"">2003</value>
                  <value id=""EndMonthValue"">08</value>
                  <value id=""EndDayValue"">24</value>
                  <value id=""EndYearValue"">2011</value>
                  <value id=""DetailsValue"">Multiple+cortisone+injections+in+the+first+8+years+after+the+initial+knee+injury.</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Fam Soc Hx"">
            <type>Family_Social_HistoryTab</type>
            <name>Fam+Soc+Hx</name>
            <panels>
              <panel id=""ff3af2e657"">
                <type>FamilyHistoryPanel</type>
                <values>
                  <value id=""FamilyHistoryValue"">Family+Health+History</value>
                  <value id=""ChronicDiseaseValue"">Relatives%3a+Mother+(age+65)+%e2%80%93+Hypertension%3b+Father+(age+65)+%e2%80%93+Hyperlipidemia%3b+Grandfather+%e2%80%93+Coronary+artery+disease%2c+deceased+from+MI+at+age+67.</value>
                </values>
              </panel>
              <panel id=""866a94900a"">
                <type>SocialHistoryPanel</type>
                <values>
                  <value id=""MaritalStatusValue"">Married</value>
                  <value id=""ChildrenValue"">2</value>
                  <value id=""SpousePartnerValue"">Ann</value>
                  <value id=""SocialSupportsValue"">Wife%2c+Ann%2c+and+a+few+%22drinking+buddies%22</value>
                  <value id=""OccupationValue"">Insurance+adjuster</value>
                </values>
              </panel>
              <panel id=""153b6b76a1"">
                <type>SubstanceUsePanel</type>
                <values>
                  <value id=""TobaccoCurrentUseToggle"">True</value>
                  <value id=""TobaccoDetailsValue"">Smokes+cigarettes.+History+of+smoking+for+28+pack%2fyears+(avg.+of+2+packs+per+day+for+14+years)%0d.+</value>
                  <value id=""AlcoholCurrentUseToggle"">True</value>
                  <value id=""AlcoholDetailsValue"">1%e2%80%932+drinks%2fday%2c+several+days+per+week</value>
                  <value id=""DrugNoneToggle"">True</value>
                  <value id=""DrugDetailsValue"">Takes+opioids+not+prescribed+for+him+that+he+obtains+from+friends+when+he+cannot+get+them+from+a+doctor.+</value>
                </values>
              </panel>
              <panel id=""bf9b58ad1e"">
                <type>DietExercisePanel</type>
                <values>
                  <value id=""DietValue"">Diet+Survey+past+due</value>
                  <value id=""ExerciseValue"">Limited+due+to+knee+pain+</value>
                </values>
              </panel>
              <panel id=""4f532a847d"">
                <type>OtherPanel</type>
                <values>
                  <value id=""CollapseButtonCheckBox"">True</value>
                  <value id=""OtherValue"">He+feels+that+the+limitations+from+his+knee+pain+and+poor+functioning+interfere+with+having+an+active+social+life+and+playing+with+his+children%2c+which+contributes+to+his+depression.</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Meds"">
            <type>Medication_HistoryTab</type>
            <name>Meds</name>
            <panels>
              <panel id=""9f6e2fb417"">
                <type>MedicationEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">oxycodone%2c+long-acting</value>
                  <value id=""StartMonthValue"">06</value>
                  <value id=""StartDayValue"">28</value>
                  <value id=""StartYearValue"">2016</value>
                  <value id=""PresentToggle"">True</value>
                  <value id=""DoseValue"">30+mg</value>
                  <value id=""HowTakenValue"">Take+one+tablet+every+12+hours</value>
                  <value id=""ConditionValue"">Chronic+knee+pain</value>
                  <value id=""ResponseValue"">Reduces+pain+in+half</value>
                </values>
                <pins>
                  <quiz>
                    <questions>
                      <panel id=""62c762d0c4"">
                        <type>QuizQuestion</type>
                        <values>
                          <value id=""QuestionValue"">Select+the+best+way+to+learn+about+all+the+prescriptions+Mr.+Wright+has+had+in+the+past+year%2c+assuming+you+have+appropriate+permissions+in+place.</value>
                          <value id=""OptionTypeValue"">Multiple+Choice</value>
                        </values>
                        <panels>
                          <panel id=""6d07ac614e"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Call+the+Pharmacy+number+he+gives+to+you</value>
                              <value id=""OptionTypeValue"">Partially+Correct</value>
                              <value id=""FeedbackValue"">Asking+the+pharmacy+(with+appropriate+permissions)+would+yield+information+about+just+that+pharmacy.+Mr.+Wright+may+omit+other+pharmacies%2c+however.+The+Prescription+Drug+Monitoring+Program+has+the+most+reliably+complete+information+on+prescriptions+in+his+name+within+the+state.+</value>
                            </values>
                          </panel>
                          <panel id=""30914371b9"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Call+his+last+prescribing+provider</value>
                              <value id=""OptionTypeValue"">Partially+Correct</value>
                              <value id=""FeedbackValue"">Asking+Mr.+Wright%27s+last+prescribing+provider+(with+appropriate+permissions)+will+only+give+you+information+about+what+that+provider+prescribed.+Contacting+the+past+provider+about+this+complex+case+is+important.+However%2c+Chad+may+have+withheld+information+about+other+doctors+he+has+seen.+%0a%0aThe+Prescription+Drug+Monitoring+Program+has+the+most+reliably+complete+information+on+prescriptions+in+his+name+within+the+state.+</value>
                            </values>
                          </panel>
                          <panel id=""f0f49a7810"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Consult+the+Prescription+Drug+Monitoring+Program</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">Prescription+Drug+Monitoring+Plan+reports+are+recommended+by+CDC%27s+opioid+prescribing+guidelines+for+all+patients+on+chronic+opioid+therapy%2c+before+prescribing+and+then+monthly+thereafter+or+at+each+prescription+(Dowell+et+al.%2c+2016).+Check+for%3a%0a+%e2%80%a2+Evidence+of+the+patient+seeing+more+than+one+doctor+for+the+same+condition%2c+which+may+signal+misuse+by+the+patient+or+diversion.+%0a+%e2%80%a2+The+patient+not+filling+a+prescription%2c+which+may+signal+diversion.+%0a%0aPrescription+drug+monitoring+programs+will+provide+prescribing+information+on+oxycodone+and+other+controlled+substances+Mr.+Wright+has+received+from+all+doctors+and+pharmacies+within+the+state.+The+other+answers+also+might+yield+some+valuable+information+but+are+less+likely+to+be+as+reliably+complete.%0a%0a%3cb%3eYour+PDMP+report+will+be+available+for+you+to+review+later+in+the+appointment.+%3c%2fb%3e</value>
                            </values>
                          </panel>
                          <panel id=""36f40895ba"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Ask+Mr.+Wright</value>
                              <value id=""OptionTypeValue"">Partially+Correct</value>
                              <value id=""FeedbackValue"">Asking+Mr.+Wright+might+gather+all+information+needed%2c+especially+if+you+have+built+an+open+and+honest+relationship+with+him.+However%2c+if+he+withholds+anything%2c+you+could+miss+important+information.+%0a%0aThe+Prescription+Drug+Monitoring+Program+has+the+most+reliably+complete+information+on+prescriptions+in+his+name+within+the+state.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                    </questions>
                  </quiz>
                </pins>
              </panel>
              <panel id=""6278fcad41"">
                <type>MedicationEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">duloxetine</value>
                  <value id=""StartMonthValue"">09</value>
                  <value id=""StartDayValue"">02</value>
                  <value id=""StartYearValue"">2016</value>
                  <value id=""PresentToggle"">True</value>
                  <value id=""DoseValue"">20+mg</value>
                  <value id=""HowTakenValue"">Take+one+tablet+once+per+day</value>
                  <value id=""ConditionValue"">Depression+and+neuropathic+pain</value>
                  <value id=""ResponseValue"">Depression+improved+from+moderate+to+mild%2c+a+little+pain+relief</value>
                </values>
              </panel>
              <panel id=""10e4d089b8"">
                <type>MedicationEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">docusate+sodium+stool+softener</value>
                  <value id=""StartMonthValue"">09</value>
                  <value id=""StartDayValue"">02</value>
                  <value id=""StartYearValue"">2016</value>
                  <value id=""PresentToggle"">True</value>
                  <value id=""DoseValue"">One+to+three+capsules</value>
                  <value id=""HowTakenValue"">Take+with+water+once+per+day</value>
                  <value id=""ConditionValue"">Opioid-induced+constipation</value>
                  <value id=""ResponseValue"">Controls+constipation+fairly+well</value>
                </values>
              </panel>
              <panel id=""4a305465b7"">
                <type>MedicationEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">oxycodone%2facetaminophen%2c+immediate-release</value>
                  <value id=""StartMonthValue"">04</value>
                  <value id=""StartDayValue"">19</value>
                  <value id=""StartYearValue"">2016</value>
                  <value id=""EndMonthValue"">06</value>
                  <value id=""EndDayValue"">27</value>
                  <value id=""EndYearValue"">2016</value>
                  <value id=""DoseValue"">10+mg%2f325+mg</value>
                  <value id=""HowTakenValue"">Take+one+tablet+every+4+to+6+hours</value>
                  <value id=""ConditionValue"">Chronic+knee+pain</value>
                  <value id=""ResponseValue"">Pain+reduced+to+a+tolerable+level+most+of+the+time</value>
                </values>
              </panel>
              <panel id=""649a29c09b"">
                <type>MedicationEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">hydrocodone%2facetaminophen</value>
                  <value id=""StartMonthValue"">11</value>
                  <value id=""StartDayValue"">09</value>
                  <value id=""StartYearValue"">2015</value>
                  <value id=""EndMonthValue"">04</value>
                  <value id=""EndDayValue"">18</value>
                  <value id=""EndYearValue"">2016</value>
                  <value id=""DoseValue"">10+mg%2f325+mg</value>
                  <value id=""HowTakenValue"">Take+one+tablet+every+4+to+6+hours</value>
                  <value id=""ConditionValue"">Chronic+knee+pain</value>
                  <value id=""ResponseValue"">Only+modest+pain+relief</value>
                </values>
              </panel>
              <panel id=""ae40c62217"">
                <type>MedicationEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">ibuprofen</value>
                  <value id=""StartMonthValue"">06</value>
                  <value id=""StartDayValue"">21</value>
                  <value id=""StartYearValue"">2011</value>
                  <value id=""PresentToggle"">True</value>
                  <value id=""DoseValue"">200+mg</value>
                  <value id=""HowTakenValue"">1-2+tabs+prn+extra+pain</value>
                  <value id=""ConditionValue"">Right+knee+chronic+pain</value>
                  <value id=""ResponseValue"">Modest+pain+relief</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Past Med Tests"">
            <type>Test_HistoryTab</type>
            <name>Past+Med+Tests</name>
            <panels>
              <panel id=""1345e2c204"">
                <type>OtherPastTestEntryPanel</type>
                <values>
                  <value id=""TestValue"">MRI%2c+right+knee</value>
                  <value id=""MonthValue"">03</value>
                  <value id=""DayValue"">07</value>
                  <value id=""YearValue"">2002</value>
                  <value id=""DetailsValue"">Tear+of+ACL%2c+right+knee</value>
                  <value id=""InterpretationValue"">ACL+rupture%2c+right+knee%2c+confirmed</value>
                  <value id=""Image"">642fb3a863</value>
                </values>
              </panel>
              <panel id=""113c73e2e3"">
                <type>OtherPastTestEntryPanel</type>
                <values>
                  <value id=""TestValue"">Metabolic+Panel</value>
                  <value id=""MonthValue"">06</value>
                  <value id=""DayValue"">24</value>
                  <value id=""YearValue"">2017</value>
                  <value id=""InterpretationValue"">No+significant+positive+findings.+</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Pain Hx"">
            <type>TextboxTab</type>
            <name>Pain+Hx</name>
            <panels>
              <panel id=""cc6abf674b"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3eInstructions%3a%3c%2fb%3e+In+order+to+obtain+a+complete+history+of+Mr.+Wright%27s+pain+and+its+treatment%2c+choose+what+the+provider+should+say+in+the+following+dialogue.+</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""3eb2ecda95"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">What+helps+the+pain%3f</value>
                        </values>
                      </panel>
                      <panel id=""6166d4c39f"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+mostly+just+take+oxycodone+for+it.+That%27s+usually+all+I+need.+On+bad+nights%2c+I+sometimes+take+the+next+dose+of+oxycodone+a+few+hours+early%2c+but+that+got+me+in+trouble.+That%27s+why+the+last+provider+stopped+prescribing+it.+So+I+was+wondering+if+you+could+prescribe+me+some+extra+pills+for+when+my+pain+gets+worse.+++</value>
                        </values>
                      </panel>
                      <panel id=""07047cede3"">
                        <type>DialogueChoiceEntry</type>
                        <panels>
                          <panel id=""62d4a6a2a7"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">%e2%80%9cI%27m+sorry%2c+with+the+dangerous+way+you%27ve+been+taking+opioids%2c+I+cannot+treat+you.%e2%80%9d</value>
                              <value id=""OptionTypeValue"">Incorrect</value>
                              <value id=""FeedbackValue"">Patients+who+struggle+with+chronic+pain+and+risky+medication+use+still+deserve+pain+treatment.+It+may+be+safe+to+treat+them+in+your+clinic+with+additional+treatment+structure.+If+his+behavior+falls+outside+the+scope+of+your+practice%2c+however%2c+he+should+be+referred+to+an+addiction+or+pain+specialist.%0d</value>
                            </values>
                          </panel>
                          <panel id=""3c4592e3f9"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">%22You+have+to+stop+managing+your+own+medications+if+you+want+to+be+treated+in+this+clinic.%22</value>
                              <value id=""OptionTypeValue"">Partially+Correct</value>
                              <value id=""FeedbackValue"">It+is+critical+that+Mr.+Wright+stop+several+dangerous+behaviors+(taking+medications+early%2c+taking+opioids+not+prescribed+for+him%2c+and+taking+an+opioid+together+with+alcohol).+These+behaviors+increase+the+risk+of+overdose%2c+which+can+be+fatal.+However%2c+the+tone+of+this+comment+was+a+little+harsh+for+starting+a+discussion.+%0a%0aAn+expression+of+concern+about+the+risks+involved+would+be+more+effective+at+creating+a+partnership+with+Mr.+Wright.+</value>
                            </values>
                          </panel>
                          <panel id=""67bf98b307"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">%22I+am+very+concerned+about+the+potential+for+an+overdose+from+taking+your+next+opioid+dose+early.+It+could+also+lead+to+addiction.%22</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">Starting+with+an+expression+of+concern+about+how+Mr.+Wright+is+taking+his+opioid+medication+before+its+time+for+another+dose+will+help+build+a+partnership+with+him.+After+making+this+connection%2c+he+may+be+more+open+to+hearing+safety+guidelines+and+your+practice%27s+policies.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                      <panel id=""bcf05d7648"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+didn%27t+think+about+that.+Thanks+for+letting+me+know.+</value>
                        </values>
                      </panel>
                      <panel id=""79f08a5192"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I+have+to+be+firm+for+the+sake+of+your+safety+and+require+that+you+to+agree+to+follow+instructions+carefully%2c+if+we+are+to+work+together+on+managing+your+pain.+But+I+will+work+with+you+to+find+the+most+effective+and+safe+combination+of+treatments+possible.+</value>
                        </values>
                      </panel>
                      <panel id=""ebbc90459a"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">All+right.+</value>
                        </values>
                      </panel>
                      <panel id=""67a2805d0f"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">What+else+have+you+tried+recently+to+treat+the+pain%3f+</value>
                        </values>
                      </panel>
                      <panel id=""07813ff8bd"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+also+take+Cymbalta%2c+the+antidepressant%2c+which+they+say+is+supposed+to+help+with+pain%2c+too.+It+might+help+a+little.+I+don%27t+like+to+use+a+cane%2c+because+it+makes+me+look+older.+I+have+two+young+children+and+I+don%27t+want+to+have+them+see+me+as+weak.+I%27d+love+to+be+more+active+with+them.</value>
                        </values>
                      </panel>
                      <panel id=""f2b8a37892"">
                        <type>DialogueChoiceEntry</type>
                        <panels>
                          <panel id=""f2fc2ecde1"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Keep+going+with+history+questions.+There%e2%80%99s+a+lot+of+information+needed+about+his+pain.+</value>
                              <value id=""OptionTypeValue"">Partially+Correct</value>
                              <value id=""FeedbackValue"">While+it+is+true+that+sometimes+you+need+to+keep+going+when+obtaining+a+history+due+to+time+limitations+and+high+medical+priorities%2c+it+is+also+important+to+acknowledge+deep+concerns+of+the+patient%27s.+You+need+to+show+that+you+are+listening+at+this+level.+It+can+be+done+briefly.+</value>
                            </values>
                          </panel>
                          <panel id=""0d22ff21d7"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Pause+to+offer+some+empathy.+</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">It+is+important+to+both+get+a+complete+history+and+offer+empathy+at+key+points+in+the+interview.+Offering+empathy+can+be+done+briefly.+Even+a+short+pause%2c+eye+contact%2c+and+a+sympathetic+sound+can+make+a+patient+feel+their+most+important+concerns+are+heard.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                      <panel id=""8bfa10d475"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">%5bPauses.+Makes+eye+contact+and+speaks+with+a+sympathetic+tone.%5d+I+understand+what+you%27re+saying.+What+about+in+the+past%3f+What+has+been+tried+to+treat+your+pain%3f</value>
                        </values>
                      </panel>
                      <panel id=""c51d903f8d"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Let%27s+see.+I+had+physical+therapy+a+long+time+ago+after+surgery%2c+but+I+it+was+painful+so+I+stopped.+They+did+another%2c+smaller+surgery+a+few+years+later+and+that+helped+a+little.+I%27ve+had+steroid+injections+which+helped+for+a+little+while.+Lortab+and+Percocet+helped+but+the+doctors+got+nervous+about+continuing+to+prescribe+opioids.+I+also+had+Toradol+and+Darvocet%2c+which+were+stopped+because+they+didn%27t+work+well+enough.+%0a%0aI+was+wondering+if+I+need+a+stronger+dose%3f+That+way+I+wouldn%27t+be+tempted+to+take+my+medication+early.</value>
                        </values>
                      </panel>
                      <panel id=""fa7d4a1f3a"">
                        <type>DialogueChoiceEntry</type>
                        <panels>
                          <panel id=""aa819faf9c"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">The+best+solution+is+a+combination+of+pain+treatments%2c+not+increasing+the+opioid+dose.</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">Current+guidelines+for+chronic+pain+management+recommend+using+a+combination+of+pain+treatments+and+limiting+opioid+use+as+much+as+possible.+</value>
                            </values>
                          </panel>
                          <panel id=""82b261db8a"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Increase+the+opioid+dose+a+little</value>
                              <value id=""OptionTypeValue"">Incorrect</value>
                              <value id=""FeedbackValue"">Increasing+the+opioid+dose+should+be+the+last+response.+First%2c+all+other+combination+of+chronic+pain+management+should+be+tried.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                      <panel id=""e9c69bd44a"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Often+the+best+solution+for+chronic+pain+is+a+combination+of+pain+treatments%2c+medications+as+well+as+other+treatments%2c+rather+than+just+raising+the+dose+of+opioids.+</value>
                        </values>
                      </panel>
                      <panel id=""3fd72a40c9"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Is+that+right%3f</value>
                        </values>
                      </panel>
                      <panel id=""14140a27e4"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">%5bNods%5d+For+me+to+think+about+what+medications+would+be+best%2c+I%27d+like+to+understand+your+risk+of+overdose.+I+need+to+know+about+other+medications+or+drugs+you+take%2c+including+those+prescribed+or+not+prescribed+for+you%2c+particularly%e2%80%a6.+</value>
                        </values>
                      </panel>
                      <panel id=""c2acdb6b34"">
                        <type>DialogueChoiceEntry</type>
                        <panels>
                          <panel id=""831c530339"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Benzodiazepines</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">Benzodiazepines+are+potentially+very+dangerous+in+combination+with+opioids.+The+combination+may+lead+to+respiratory+distress+and+potentially+a+fatal+overdose.+A+high+level+of+caution+should+be+employed+with+combining+opioids+with+them+or+other+medications+that+cause+drowsiness+or+relaxation.</value>
                            </values>
                          </panel>
                          <panel id=""7b3bcb3ebb"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Cocaine</value>
                              <value id=""OptionTypeValue"">Partially+Correct</value>
                              <value id=""FeedbackValue"">Drug+interactions+between+opioids+and+cocaine+are+not+particularly+dangerous.+However%2c+it+is+worth+noting+that+individuals+who+use+illegal+drugs%2c+on+average%2c+are+at+higher+risk+for+opioid+abuse.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                      <panel id=""d14c523242"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">...particularly+benzodiazepines%2c+such+as+Xanax+or+Valium%2c+or+other+medications+that+make+you+drowsy+or+relaxed%3f</value>
                        </values>
                      </panel>
                      <panel id=""ac9ce99028"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">No%2c+just+oxycodone+and+my+antidepressant.</value>
                        </values>
                      </panel>
                      <panel id=""a0a32bb956"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Thank+you+for+answering+my+questions+and+helping+me+understand+what+you%27re+experiencing.+I+have+a+couple+of+quick+surveys+that+will+help+me+in+coming+up+with+a+plan+to+manage+your+pain.</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
            </panels>
          </tab>
          <tab id=""Differential Dx"">
            <type>Differential_DiagnosisTab</type>
            <name>Differential+Dx</name>
            <panels>
              <panel id=""f0ca9afb23"">
                <type>SymptomClusterEntryPanel</type>
                <values>
                  <value id=""PriorityValue"">Primary+Diagnosis</value>
                  <value id=""SymptomClusterValue"">Aberrant+drug-related+behavior+(taking+medication+without+a+prescription)</value>
                  <value id=""FeedbackValue"">1.+Mr.+Wright+explains+that+he+still+has+pain+and+that+this+is+the+reason+he+took+the+medication+early%2c+so+this+explanation+makes+sense+at+face+value.+However%2c+the+other+diagnoses+should+also+be+considered.+%0a%0a2.+Not+following+his+prescription%27s+instructions+is+worrisome+behavior+and+is+one+risk+factor+for+possible+opioid+use+disorder.+Other+risks+should+be+explored.+%0a%0a3.+When+a+patient+needs+more+medication+than+prescribed%2c+even+if+they+have+a+plausible+explanation%2c+diversion+should+be+considered+as+a+possible+reason.+%0a%0a4.+A+cognitive+impairment+could+lead+to+a+patient+taking+his+medication+earlier+than+scheduled%2c+however%2c+Mr.+Wright+has+already+explained+that+he+did+it+intentionally.+Also%2c+he+has+no+other+signs+of+cognitive+impairment.+</value>
                </values>
                <panels>
                  <panel id=""9103a2dfc1"">
                    <type>DiagnosisEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Under-treated+chronic+pain</value>
                    </values>
                  </panel>
                  <panel id=""6644f02143"">
                    <type>DiagnosisEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Opioid+use+disorder</value>
                    </values>
                  </panel>
                  <panel id=""f3de350fe5"">
                    <type>DiagnosisEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Diversion</value>
                    </values>
                  </panel>
                  <panel id=""b53f60c5c1"">
                    <type>DiagnosisEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Cognitive+impairment</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
      <section id=""_EvaluationSection"">
        <name>_EvaluationSection</name>
        <tabs>
          <tab id=""Evaluation Info"">
            <type>TextboxTab</type>
            <name>Evaluation+Info</name>
            <panels>
              <panel id=""d40824c3ce"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3e%3ccolor%3d%23F26F45%3eBACKGROUND+INFORMATION+FOR+EVALUATION%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cb%3eEvaluating+Patients+with+Chronic+Pain+%3c%2fb%3e+%0a%0aIt+is+important+to+evaluate+patients+having+pain+for+psychological+or+social+factors+that+could+affect+their+recovery.+%0a%0a%3cb%3eAssessing+Depression+%3c%2fb%3e+%0a%0aIdentifying+and+treating+depression+is+an+important+component+of+pain+management+because+it+can+have+a+direct+effect+on+the+experience+of+pain.+The+interactions+between+pain+and+depression+are+complex+with+causality+going+in+both+directions%3a+Pain+contributes+to+depression%2c+and+depression+contributes+to+pain.+%0a%0aDepression+is+the+most+common+mood+disorder+among+patients+with+opioid+use+disorder.+An+estimated+44%25+to+54%25+of+patients+with+opioid+use+disorder+have+suffered+from+major+depression+at+some+point+in+their+lifetime+(Pani+et+al.%2c+2010).+Up+to+30%25+of+patients+with+opioid+use+disorder+are+currently+depressed.+%0a%0aHigher+dose+opioids+are+associated+with+increased+symptoms+of+depression.+However%2c+increased+symptoms+of+depression+may+be+due+to+the+high+dose+opioids+or+greater+pain+that+resulted+in+high+dose+opioids+being+prescribed+(Merril+et+al.%2c+2012).+Treatment+for+depression%2c+for+example%2c+cognitive+behavioral+therapy+(CBT)%2c++often+improves+pain.+CBT+may+include+learning+pain+coping+strategies.+A+patient%27s+affect+and+mood+may+also+benefit+from+relaxation+strategies+and+biofeedback.+Traditional+antidepressant+therapy+also+may+help+(Briley+%26+Moret%2c+2008).+%0a%0a%3cb%3eAssessing+Physical+and+Psychosocial+Functioning+%3c%2fb%3e+%0a%0d%0aFunctioning+is+the+impact+of+the+pain+on+the+patient%27s+life.+Include+these+areas+in+your+questions%3a%0d%0a%0d%0a+%e2%80%a2++Psychological+Functioning%2fMood%3a+Does+the+pain+affect+your+mood+or+ability+to+enjoy+life%3f+Note%3a+May+need+to+involve+caregivers+in+this+discussion.%0d%0a%0d%0a+%e2%80%a2+Daily+Activities%3a+Does+your+pain+keep+you+from+doing+anything%2c+such+as+daily+activities%3f+(e.g.%2c+sleeping%2c+walking%2c+cleaning%2c+shopping%2c+work%2c+play%2c+personal+hygiene%2c+childcare%2c+or+hobbies).%0d%0a%0d%0a+%e2%80%a2+Social+Functioning%3a+Does+the+pain+affect+your+relationships%3f%0d%0a(Marshall%2c+2010)%0d%0a%0d%0aAnother+assessment+relevant+to+pain+management+is+the+PEG+scale%2c+which+is+a+quick+assessment+of+pain%2c+functioning%2c+and+impact+of+pain+on+the+patient%27s+life+(Krebs+et+al.%2c+2009).%0d%0a%0a%3cb%3eScreening+for+Opioid+Risk+%3c%2fb%3e+%0aScreening+surveys+may+help+evaluate+a+patient+for+their+risk+if+prescribed+opioids.+The+biggest+risk+factor+is+having+a+history+of+substance+abuse.+Example+screening+survey%3a+%0d%0a%0d%0a%3cb%3eOpioid+Risk+Tool+(ORT)+%3c%2fb%3e+(Webster%2c+2005)%0d%0aQuestions+cover%3a%0d%0a1.+Family+history+of+substance+abuse%0d%0a2.+Personal+history+of+substance+abuse%0d%0a3.+Age+(16-45+has+the+highest+risk)%0d%0a4.+History+of+preadolescent+sexual+abuse+(for+females+only)%0d%0a5.+Psychological+Disease%0d%0aAlternative+screening+survey%3a+Diagnosis%2c+Intractability%2c+Risk%2c+Efficacy+(DIRE)+(Belgrade%2c+2006)%0d%0a%0a%3cb%3ePrescription+Drug+Monitoring%3c%2fb%3e%0aConsult+prescription+drug+monitoring+program+database+before+prescribing+opioids+and+during+treatment%3a+Look+at+total+opioid+doses+and+dangerous+drug+combinations.+Check+database+at+least+every+3+months+and+consider+checking+at+every+prescription.%0d%0a%0aRecommendations+from%3a+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016+(Dowell%2c+et+al.%2c+2016)%0a</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Substance Use"">
            <type>Screen_AssessTab</type>
            <name>Substance+Use</name>
            <panels>
              <panel id=""966df0c1fe"">
                <type>ScreenAssessEntryPanel</type>
                <values>
                  <value id=""SurveyTitleValue"">CAGE-AID</value>
                  <value id=""SurveyInstructionValue"">When+thinking+about+drug+use%2c+include+illegal+drug+use+and+the+use+of+prescription+drug+other+than+prescribed.</value>
                  <value id=""ScoreValue"">3+points</value>
                  <value id=""ScoreKeyValue"">A+score+of+2+or+more+points+is+positive.+A+score+of+1+point+is+also+positive+if+it+is+for+the+last+question.</value>
                  <value id=""InterpretationValue"">Positive+screen.++Further+assessment+is+indicated.</value>
                </values>
                <panels>
                  <panel id=""a2e37fc75b"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Have+you+ever+felt+you+ought+to+cut+down+on+your+drinking+or+drug+use%3f</value>
                      <value id=""AnswerValue"">Yes%2c+but+only+opioids+</value>
                    </values>
                  </panel>
                  <panel id=""a9eb9cfa4d"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Have+people+annoyed+you+by+criticizing+your+drinking+or+drug+use%3f</value>
                      <value id=""AnswerValue"">No</value>
                    </values>
                  </panel>
                  <panel id=""c4a3e1ef8e"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Have+you+ever+felt+bad+or+guilty+about+your+drinking+or+drug+use%3f</value>
                      <value id=""AnswerValue"">Yes%2c+but+only+for+taking+extra+opioids</value>
                    </values>
                  </panel>
                  <panel id=""1d3ddde3ee"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Have+you+ever+had+a+drink+or+drug+first+thing+in+the+morning+to+steady+your+nerves+or+to+get+rid+of+a+hangover%3f</value>
                      <value id=""AnswerValue"">Yes</value>
                    </values>
                  </panel>
                </panels>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""09ddaa1a08"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I%27m+interested+in+learning+more+about+your+responses+to+these+surveys.+For+example%2c+you+said+you+feel+you+should+cut+back+on+your+alcohol+use%2c+too.+Can+you+say+more+about+that%3f++</value>
                        </values>
                      </panel>
                      <panel id=""bc8376ab43"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Sure.+I+don%27t+drink+much+-+just+1+or+2+beers+a+few+times%2c+maybe+4+times+a+week+at+the+most%2c+but+the+doctor+who+prescribed+my+antidepressant+said+I+shouldn%27t+drink+alcohol+while+I+take+it+or+it+could+hurt+my+liver.+And+the+other+doctor+said+that+I%27m+not+supposed+to+mix+alcohol+and+opioids.+But+I+sometimes+drink+anyhow+when+I%27m+feeling+a+little+depressed+or+my+pain+is+bad.</value>
                        </values>
                      </panel>
                      <panel id=""b5450270e0"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Yes%2c+alcohol+can+have+harmful+effects+together+with+your+antidepressant+and+using+alcohol+with+opioids+increases+your+risk+of+overdose+and+problems+driving+or+operating+machinery.+%0a%0aWe%27ll+need+to+maximize+your+pain+control%2c+but+it+sounds+like+it+would+also+help+to+get+some+help+with+more+pain+coping+skills+and+remaining+depression%2c+so+that+you+won%27t+want+to+turn+to+drinking.</value>
                        </values>
                      </panel>
                      <panel id=""8a664f2276"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">That+sounds+good.</value>
                        </values>
                      </panel>
                      <panel id=""b382278123"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">You+say+you+sometimes+take+something+first+thing+in+the+morning+to+steady+your+nerves.+Is+that+alcohol+or+oxycodone%3f</value>
                        </values>
                      </panel>
                      <panel id=""eb9d03a7f2"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Just+oxycodone.+If+I+don%27t+take+my+it%2c+I+get+shaky+by+mid-morning.+That%27s+withdrawal%2c+I+guess.</value>
                        </values>
                      </panel>
                      <panel id=""ce3f38531d"">
                        <type>DialogueChoiceEntry</type>
                        <panels>
                          <panel id=""d23066a7e0"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Yes%2c+that+sounds+like+withdrawal.</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">Yes%2c+feeling+%e2%80%9cshaky%e2%80%9d+by+mid-morning+after+not+having+an+opioid+dose+since+the+previous+day+is+a+symptom+of+opioid+withdrawal.+Other+symptoms+of+opioid+withdrawal+are%3a+%0a%c2%a0%e2%80%a2%c2%a0+Dysphoric+mood%0d%0a%c2%a0%e2%80%a2%c2%a0+Lacrimation+or+rhinorrhea%0d%0a%c2%a0%e2%80%a2%c2%a0+Fever</value>
                            </values>
                          </panel>
                          <panel id=""9cab21444f"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">No%2c+that%27s+not+withdrawal.</value>
                              <value id=""OptionTypeValue"">Incorrect</value>
                              <value id=""FeedbackValue"">Feeling+%e2%80%9cshaky%e2%80%9d+by+mid-morning+after+not+having+an+opioid+dose+since+the+previous+day+is+a+symptom+of+opioid+withdrawal.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                      <panel id=""7f38ef2b7f"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">It+sounds+like+it+could+be+withdrawal.+People+who+take+opioids+for+chronic+pain+do+become+physically+dependent+on+them+and+get+withdrawal+symptoms+when+they+go+for+a+while+without+any+opioids.+</value>
                        </values>
                      </panel>
                      <panel id=""9525a75d1c"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Yes%2c+that%27s+what+happens.+</value>
                        </values>
                      </panel>
                      <panel id=""83161254a7"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">And+you+said+that+you+feel+you+should+cut+down+on+substance+use+and+feel+some+guilt+about+your+use.++Did+you+mean+opioids%2c+alcohol%2c+or+other+substances%3f+</value>
                        </values>
                      </panel>
                      <panel id=""89449b8c63"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+meant+oxycodone%2c+for+one.+I+feel+like+I+need+it+even+when+the+pain+is+better+and+I+don%27t+like+that.+So%2c+I+feel+guilty+and+want+to+cut+back.+But+I%27ve+struggled+with+that.</value>
                        </values>
                      </panel>
                      <panel id=""3edc50f9db"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I%27d+like+to+work+with+you+to+help+you+with+your+need+for+opioids.+With+today%27s+treatment+guidelines%2c+you+would+not+have+been+kept+on+opioids+for+this+long.+I+can%27t+say+for+sure+about+your+situation+and+sometimes+opioids+are+needed%2c+but+current+treatment+recommendations+are+for+3+days+of+opioids+or+at+the+most%2c+one+week%2c+for+acute+pain.++</value>
                        </values>
                      </panel>
                      <panel id=""bfb75a9493"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+wish+I+knew+that+back+then%e2%80%a6I+also+said+%22yes%22+to+those+questions+because+I+feel+guilty+about+drinking+alcohol.</value>
                        </values>
                      </panel>
                      <panel id=""ef3ab9817d"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Would+you+consider+going+to+counseling+again%3f+I+could+help+you+find+someone+who+specializes+in+pain+coping+skills+and+helping+with+depression.+</value>
                        </values>
                      </panel>
                      <panel id=""01ead36e54"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">That+sounds+good.+I+might+be+interested%2c+depending+upon+costs+and+all+that.</value>
                        </values>
                      </panel>
                      <panel id=""3c2f40c15e"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">That+sounds+good.+I+might+be+interested%2c+depending+upon+costs+and+all+that.</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
            </panels>
          </tab>
          <tab id=""Other Assessments"">
            <type>Screen_AssessTab</type>
            <name>Other+Assessments</name>
            <panels>
              <panel id=""4f77f9d7f1"">
                <type>ScreenAssessEntryPanel</type>
                <values>
                  <value id=""SurveyTitleValue"">Beck+Depression+Inventory+(Beck+et+al.%2c+1988)</value>
                  <value id=""ScoreValue"">7+points.+This+was+due+to+Mr.+Wright+giving+mild%2c+positive+responses+to+symptoms%3a+sadness%2c+pessimism%2c+guilt%2c+self-dislike%2c+irritability%2c+work+difficulty%2c+and+fatigability.</value>
                  <value id=""ScoreKeyValue"">A+score+of+0-13+is+considered+minimal+depression.</value>
                  <value id=""InterpretationValue"">Minimal+depression</value>
                </values>
              </panel>
              <panel id=""0062f817f6"">
                <type>ScreenAssessEntryPanel</type>
                <values>
                  <value id=""SurveyTitleValue"">PEG+Scale+(Krebs+et+al.%2c+2009)+Functioning+Screening</value>
                  <value id=""ScoreValue"">Mr.+Wright%27s+raw+score+was+14.+Divided+by+3+equals+a+final+score+of+4.7.</value>
                  <value id=""ScoreKeyValue"">The+PEG+Scale+is+used+primarily+to+track+changes+in+pain+and+functioning+over+time.</value>
                  <value id=""InterpretationValue"">The+total+score+is+divided+by+10.+Mr.+Wright%27s+results+are+his+total+score+of+14+divided+by+3+which+equals+4.7+out+of+a+total+possible+score+of+10.+This+is+his+baseline+Peg+Scale+result+and+should+be+compared+to+future+results.+A+change+for+the+individual+patient+is+a+measure+of+pain+and+functioning+response+to+treatment.+Repeat+at+each+appointment.</value>
                </values>
                <panels>
                  <panel id=""37e50f8cdb"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Average+pain%3f</value>
                      <value id=""AnswerValue"">5+out+of+10</value>
                    </values>
                  </panel>
                  <panel id=""b2077c3c55"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Pain+interference+with+enjoyment+of+life%3f</value>
                      <value id=""AnswerValue"">6+out+of+10</value>
                    </values>
                  </panel>
                  <panel id=""805d3b8cfa"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Pain+interference+with+general+activity%3f</value>
                      <value id=""AnswerValue"">3+out+of+10</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
          <tab id=""Phys Ex"">
            <type>Physical_ExamTab</type>
            <name>Phys+Ex</name>
            <panels>
              <panel id=""5169cda13f"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">General+Appearance</value>
                  <value id=""CollapseButtonRadio"">True</value>
                  <value id=""NotesValue"">Well-nourished%2c+hyperactive+(self-report)</value>
                </values>
              </panel>
              <panel id=""e2af1f80da"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">HEENT</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
              <panel id=""d8b1eccb00"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Neck</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
              <panel id=""fd953f3625"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Musculoskeletal</value>
                  <value id=""CollapseButtonRadio"">True</value>
                  <value id=""NotesValue"">No+swelling+or+redness+of+right+knee+or+any+other+joints.+See+%22Extremities%22+for+a+description+of+knee+functionality+limitations.+</value>
                </values>
              </panel>
              <panel id=""73acbef325"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Lungs</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
              <panel id=""0c3719ac2c"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Heart</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
              <panel id=""c11d0742b2"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Neurological%2fPsych</value>
                  <value id=""CollapseButtonRadio"">True</value>
                  <value id=""NotesValue"">Right+knee%3a+%0a%c2%a0%e2%80%a2%c2%a0+Marked+crepitus+throughout+range+of+motion%0a%c2%a0%e2%80%a2%c2%a0+Severe+pain+elicited+by+ascending+and+descending+stairs%0a%c2%a0%e2%80%a2%c2%a0+Mild+to+moderate+pain+with+both+passive+and+active+motions%0a%c2%a0%e2%80%a2%c2%a0+Mild+atrophy+and+weakness+of+quadriceps%0a%c2%a0%e2%80%a2%c2%a0+Ligament+tests+unremarkable%0a%c2%a0%e2%80%a2%c2%a0+Normal+range+of+motion%0aOther+joints+WNL</value>
                </values>
              </panel>
              <panel id=""f9749b8017"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Extremities</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
              <panel id=""f10de4d8d8"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Skin</value>
                  <value id=""CollapseButtonRadio"">True</value>
                  <value id=""NotesValue"">No+rashes+or+infections</value>
                </values>
              </panel>
              <panel id=""5b6af393f4"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Rectum</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
              <panel id=""3a45ff9193"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Abdomen</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
              <panel id=""c835fd28d9"">
                <type>PhysicalExamEntryPanel</type>
                <values>
                  <value id=""PanelNameValue"">Genitourinary</value>
                  <value id=""WithinNormalLimitsToggle"">True</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""PDMP Report"">
            <type>TextboxTab</type>
            <name>PDMP+Report</name>
            <panels>
              <panel id=""dd7fbbc349"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3ePrescription+Drug+Monitoring+Report%3c%2fb%3e%0a%0a%3cb%3ePatient%3a%3c%2fb%3e+Chad+Wright%0a%0a%3cb%3eSummary%3a+%3c%2fb%3e%0a+%e2%80%a2+Mr.+Wright+received+a+prescription+for+extended-release+oxycodone+starting+a+little+over+a+year+ago%2c+that+ran+out+yesterday.++%0a+%e2%80%a2+Initially%2c+he+was+prescribed+immediate-release+oxycodone%2c+the+dose+was+gradually+increased%2c+and+then+he+was+switched+to+extended-release+oxycodone.+%0a+%e2%80%a2+Earlier%2c+he+was+prescribed+oxycodone+(10+mg)+plus+acetaminophen+(325+mg)+for+almost+a+year+by+a+different+provider.+%0d%0a+%e2%80%a2+Although+he+has+switched+doctors+a+number+of+times%2c+he+only+sees+one+doctor+at+a+time+and+only+uses+one+pharmacy.+</value>
                </values>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
      <section id=""_Medical_TestsSection"">
        <name>_Medical_TestsSection</name>
        <tabs>
          <tab id=""Medical Tests Info"">
            <type>TextboxTab</type>
            <name>Medical+Tests+Info</name>
            <panels>
              <panel id=""6603826505"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3ccolor%3d%2333CCCC%3e%3cb%3eGUIDELINES+FOR+MEDICAL+TESTS+RELEVANT+TO+THIS+CASE%3c%2fb%3e%3c%2fcolor%3e%3cb%3e%0a%0aFrom+CDC+Guidelines+for+Prescribing+Opioids+for+Chronic+Pain+(Dowell+et+al.%2c+2016%3c%2fb%3e)%0a%0aGuidelines+for+using+opioids+to+treat+chronic+pain+include+the+following+recommendations%3a%0a%3cb%3eUrine+Drug+Testing%3c%2fb%3e%0aUse+urine+drug+testing+before+and+during+opioid+treatment+at+least+annually.+Test+for+the+prescribed+medications%2c+controlled+prescription+drugs%2c+and+illicit+drugs.+Frequency+varies+with+individual+clinician%2fclinic+and%2for+patient+situation.%0d%0a%0a%3cb%3ePrescription+Drug+Monitoring%3c%2fb%3e%0aConsult+prescription+drug+monitoring+program+database+before+prescribing+opioids+and+during+treatment%3a+Look+at+total+opioid+doses+and+dangerous+drug+combinations.+Check+database+at+least+every+3+months+and+consider+checking+at+every+prescription.%0d%0a%0aRecommendations+from%3a+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016+(Dowell%2c+et+al.%2c+2016)%0d%0a%0a%3cb%3eImaging+and+Osteoarthritis+of+the+Knee%3c%2fb%3e%0d%0aRadiographic+changes+that+might+indicate+osteoarthritis+in+the+knee+include+the+presence+of+osteophytes%2c+which+are+an+overgrowth+of+bone+in+response+to+the+chronic+inflammation%2c+and+joint+space+narrowing+(Kellgren+%26+Lawrence%2c+2000)%0a</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Radiology"">
            <type>Medical_TestsTab</type>
            <name>Radiology</name>
            <panels>
              <panel id=""f027c602f3"">
                <type>Other+Medical+Tests</type>
                <values>
                  <value id=""TestValue"">MRI</value>
                  <value id=""DetailsValue"">Minor+calcifications+in+the+joint+space+near+ACL+insertion%2c+right+knee</value>
                  <value id=""InterpretationValue"">Patellofemoral+and+tibiofemoral+osteoarthritis%3a+possible+osteophyte+lipping+and+possible+joint+space+narrowing+on+anteroposterior+view%2c+right+knee.</value>
                  <value id=""Image"">0fb8b554cf</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">It+is+important+to+evaluate+the+current+status+of+pain+conditions+periodically+in+patients+on+chronic+opioid+therapy+to+help+determine+if+opioids+are+still+indicated.+His+last+MRI+was+16+years+ago%2c+so+another+one+was+indicated.</value>
                </values>
              </panel>
              <panel id=""06b09c4e17"">
                <type>Other+Medical+Tests</type>
                <values>
                  <value id=""TestValue"">No+imaging+indicated+at+this+time</value>
                  <value id=""DetailsValue"">No+imaging+results+with+this+choice</value>
                  <value id=""OptionTypeValue"">Incorrect</value>
                  <value id=""FeedbackValue"">It+is+important+to+evaluate+the+current+status+of+pain+conditions+periodically+in+patients+on+chronic+opioid+therapy+to+help+determine+if+opioids+are+still+indicated.+His+last+MRI+was+16+years+ago%2c+so+another+one+was+indicated.+</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Lab Tests"">
            <type>Medical_TestsTab</type>
            <name>Lab+Tests</name>
            <panels>
              <panel id=""7a264fff17"">
                <type>Single+Lab+Test</type>
                <values>
                  <value id=""TestNameValue"">Urine+Toxicology</value>
                  <value id=""WNLToggle"">True</value>
                  <value id=""ValueValue"">Positive+for+some+drugs+tested</value>
                  <value id=""InterpretationValue"">Positive+for+oxycodone%2c+oxymorphone.+Clear+for+all+other+drugs+tested+(marijuana+metabolites%2c+cocaine+metabolites%2c+other+opioids+and+metabolites%2c+phencyclidine%2c+amphetamines).</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Urine+drug+tests+are+indicated+before+prescribing+opioids+and+periodically+during+chronic+opioid+therapy.+Mr.+Wright%e2%80%99s+urine+drug+test+results+are+what+would+be+expected+for+someone+taking+oxycodone%2c+that+is%2c+they+include+oxycodone+and+a+metabolite.+</value>
                </values>
              </panel>
              <panel id=""fc25371aec"">
                <type>Single+Lab+Test</type>
                <values>
                  <value id=""TestNameValue"">Routine+urinalysis</value>
                  <value id=""WNLToggle"">True</value>
                  <value id=""ValueValue"">None</value>
                  <value id=""InterpretationValue"">Test+not+completed%3b+no+medical+indication+for+it+in+this+case.</value>
                  <value id=""OptionTypeValue"">Partially+Correct</value>
                  <value id=""FeedbackValue"">Urinalysis+could+be+indicated+depending+upon+the+patient%27s+medical+status+and+need+for+general+physical+evaluation.+Mr.+Wright+had+no+indications+of+other+medical+problems.</value>
                </values>
              </panel>
              <panel id=""ceccc8b127"">
                <type>Single+Lab+Test</type>
                <values>
                  <value id=""TestNameValue"">No+lab+tests+indicated</value>
                  <value id=""WNLToggle"">True</value>
                  <value id=""ValueValue"">None</value>
                  <value id=""OptionTypeValue"">Incorrect</value>
                  <value id=""FeedbackValue"">Urine+drug+tests+are+indicated+before+prescribing+opioids+and+periodically+during+chronic+opioid+therapy.+Mr.+Wright%e2%80%99s+urine+drug+test+results+are+what+would+be+expected+for+someone+taking+oxycodone%2c+that+is%2c+they+include+oxycodone+and+a+metabolite.+</value>
                </values>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
      <section id=""_DiagnosisSection"">
        <name>_DiagnosisSection</name>
        <tabs>
          <tab id=""Diagnosis Info"">
            <type>TextboxTab</type>
            <name>Diagnosis+Info</name>
            <panels>
              <panel id=""bce7ebac05"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3e%3ccolor%3d%23EB4D5C%3eBACKGROUND+INFORMATION+FOR+DIAGNOSIS%3c%2fcolor%3e%3c%2fb%3e%0a%3cb%3eDiagnostic+Criteria+for+Opioid+Use+Disorder%3c%2fb%3e%0a%0aA+diagnosis++of+Opioid+Use+Disorder%2c+according+to+the+DSM-5%2c+requires+a+pattern+of+using+opioids+causing+clinically+significant+impairment+or+distress+and+meeting+at+least+2+of+the+following+criteria%3a%0a%0a+%e2%80%a2+Taking+the+opioid+in+larger+amounts+and+for+longer+than+intended%0a+%e2%80%a2+Wanting+to+cut+down+or+quit+but+not+being+able+to+do+it%0a+%e2%80%a2+Spending+a+lot+of+time+obtaining+the+opioid+%0a+%e2%80%a2+Craving+or+a+strong+desire+to+use+opioids%0a+%e2%80%a2+Repeatedly+being+unable+to+carry+out+major+obligations+at+work%2c+school%2c+or+home+due+to+opioid+use%0a+%e2%80%a2+Continuing+use+despite+it+causing+persistent+or+recurring+social+or+interpersonal+problems%0a+%e2%80%a2+Stopping+or+reducing+important+social%2c+occupational%2c+or+recreational+activities+due+to+opioid+use%0a+%e2%80%a2+Recurrently+using+opioids+in+physically+hazardous+situations%0a+%e2%80%a2+Consistently+using+opioids+despite+it+causing+persistent+or+recurrent+physical+or+psychological+difficulties%0a+%e2%80%a2+Being+tolerant+for+opioids+(needing+increased+amounts+to+achieve+the+desired+effect+or+experiencing+diminished+effect+with+the+same+amount)%0a+%e2%80%a2+Experiencing+withdrawal+or+using+the+substance+to+avoid+withdrawal.+The+criteria+of+tolerance+and+withdrawal+do+not+apply+when+the+opioid+is+being+used+appropriately+under+medical+supervision.%0a%0aThe+above+criteria+are+paraphrased+from+the+DSM+5+(PCSS+reproduction+of+APA%2c+2013)</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Diagnosis Rounds"">
            <type>QuizTab</type>
            <name>Diagnosis+Rounds</name>
            <panels>
              <panel id=""8c2315bb41"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">Mr.+Wright+is+being+treated+for+both+chronic+pain+and+depression.+%0a%0aChoose+the+best+description+for+the+interaction+between+chronic+depression+and+chronic+pain%3f+(Choose+all+that+apply)</value>
                  <value id=""OptionTypeValue"">Checkboxes</value>
                </values>
                <panels>
                  <panel id=""4487e02898"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Chronic+depression+contributes+to+chronic+pain</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Chronic+depression+contributes+to+chronic+pain+and+vice+versa.</value>
                    </values>
                  </panel>
                  <panel id=""36deb26d44"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Chronic+pain+contributes+to+depression</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Chronic+depression+contributes+to+chronic+pain+and+vice+versa.</value>
                    </values>
                  </panel>
                  <panel id=""994e96d3c9"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Neither+statement+above+is+true</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Both+statements+are+true+in+a+complex+interaction%3a+Chronic+depression+contributes+to+chronic+pain+and+chronic+pain+contributes+to+depression.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
              <panel id=""944a716c02"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">True+or+False.+A+diagnosis+of+Opioid+Use+Disorder+should+be+reserved+for+individuals+having+a+pattern+of+opioid+use+causing+%22clinically+significant+impairment+or+distress%22+and+meeting+5+or+more+diagnostic+criteria.</value>
                  <value id=""OptionTypeValue"">Multiple+Choice</value>
                </values>
                <panels>
                  <panel id=""e0afacf6bc"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">True</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Instead+of+5+criteria%2c+only+2+or+more+diagnostic+criteria+are+required+for+this+diagnosis.+The+diagnosis+is+further+specified+according+to+how+many+criteria+are+met.+Having+2-3+criteria+met+is+considered+mild+opioid+use+disorder%2c+4-5+criteria+is+moderate%2c+and+6+or+more+criteria+is+severe.</value>
                    </values>
                  </panel>
                  <panel id=""083c168e0b"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">False</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Instead+of+5+criteria%2c+only+2+or+more+diagnostic+criteria+are+required+for+this+diagnosis.+The+diagnosis+is+further+specified+according+to+how+many+criteria+are+met.+Having+2-3+criteria+met+is+considered+mild+opioid+use+disorder%2c+4-5+criteria+is+moderate%2c+and+6+or+more+criteria+is+severe.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
          <tab id=""Diagnosis"">
            <type>DiagnosisTab</type>
            <name>Diagnosis</name>
            <panels>
              <panel id=""e2d6f49a1a"">
                <type>DiagnosisEntryPanel</type>
                <values>
                  <value id=""PriorityValue"">Primary+Diagnosis</value>
                  <value id=""SymptomClusterValue"">Chronic+pain+and+limited+functioning%2c+right+knee</value>
                </values>
                <panels>
                  <panel id=""bb9bed7ed6"">
                    <type>DxEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Crystalline+joint+disease%2c+right+knee</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">This+diagnosis+should+be+included+in+the+differential%2c+however%2c+it+seems+unlikely.+Crystalline+joint+disease%2c+such+as+gout+would+typically+have+an+acute+onset%2c+present+with+redness+and+swelling%2c+and+affect+multiple+other+joints+as+would+many+other+forms+of+arthritis.+Gout+also+would+often+have+a+history+of+toe+involvement+which+is+missing+in+this+case.</value>
                    </values>
                  </panel>
                  <panel id=""e501cf82c3"">
                    <type>DxEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Osteoarthritis+and+joint+fibrosis%2c+right+knee</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">This+diagnosis+was+confirmed+by+recent+MRI+and+is+supported+by+symptoms+and+history.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
              <panel id=""a62e1b0f0d"">
                <type>DiagnosisEntryPanel</type>
                <values>
                  <value id=""PriorityValue"">Secondary+Diagnosis</value>
                  <value id=""SymptomClusterValue"">Opioid+tolerance%2c+withdrawal%2c+positive+CAGE-AID+screening+for+opioid+use%2c+aberrant+drug-related+behavior+(taking+medication+without+a+prescription)%2c+and+several+risk+factors+for+substance+use+problems.</value>
                </values>
                <panels>
                  <panel id=""a8defe32af"">
                    <type>DxEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Opioid+use+problem+needing+further+evaluation</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Mr.+Wright+needs+further+evaluation+because+it%27s+not+clear+whether+he+meets+the+description+and+criteria+for+the+diagnosis+of+opioid+use+disorder.+%0d%0a%0d%0aThe+description+for+%e2%80%9copioid+use+disorder%e2%80%9d+is+%e2%80%9ca+pattern+of+using+opioids+causing+clinically+significant+impairment+or+distress.%e2%80%9d+Mr.+Wright+would+also+have+to+meet+at+least+two+of+the+diagnostic+criteria+to+have+this+diagnosis.+He+currently+meets+these+diagnostic+criterion%3a+wanting+to+cut+down+or+quit+but+not+being+able+to%2c+withdrawal+symptoms+when+he+does+not+take+opioids%2c+and+tolerance+(needing+increased+amount+of+the+medication+for+the+same+effect)+but+the+last+two+criteria+are+not+counted+for+a+person+being+treated+for+pain+with+chronic+opioid+therapy.+%0dSee+Diagnosis+Information+for+a+full+list+of+the+diagnostic+criteria.+%0a%0d%0aFurthermore%2c+his+behavior+might+be+a+response+to+unmanaged+pain.+His+pain+should+be+managed+and+then+the+possibility+of+opioid+use+disorder+can+be+re-visited.+%0d%0a</value>
                    </values>
                  </panel>
                  <panel id=""6b0f53af1a"">
                    <type>DxEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Unmanaged+pain%2c+no+opioid+use+disorder</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">It+is+possible+that+his+behavior+could+be+explained+entirely+by+a+response+to+unmanaged+pain.+Once+his+pain+is+better+managed%2c+the+possibility+of+opioid+use+disorder+can+be+re-assessed.</value>
                    </values>
                  </panel>
                  <panel id=""48fd2eac6c"">
                    <type>DxEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Opioid+use+disorder</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Mr.+Wright+needs+further+evaluation+for+the+diagnosis+of+opioid+use+disorder.++%0d%0aIt+is+not+clear+whether+he+meets+the+description+for+opioid+use+disorder%2c+which+is%3a+%0a%0a%22A+pattern+of+using+opioids+causing+%22clinically+significant+impairment+or+distress.%22+Also%2c+to+have+this+diagnosis%2c+he+would+have+to+meet+at+least+two+of+the+diagnostic+criteria.+He+currently+meets+one+diagnostic+criterion%2c+%22Wanting+to+cut+down+or+quit+but+not+being+able+to+do+it.%e2%80%9d+He+does+experience+two+other+criteria%3a+withdrawal+symptoms+when+he+does+not+take+opioids+and+tolerance+(needing+increased+amount+of+the+medication+for+the+same+effect)%2c+but+these+two+criteria+are+not+counted+for+a+person+being+treated+for+pain+with+chronic+opioid+therapy.+See+Diagnosis+Information+for+a+full+list+of+the+diagnostic+criteria.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
              <panel id=""7d25bc8836"">
                <type>DiagnosisEntryPanel</type>
                <values>
                  <value id=""PriorityValue"">Secondary+Diagnosis</value>
                  <value id=""SymptomClusterValue"">Constipation</value>
                </values>
                <panels>
                  <panel id=""c9f28d7eac"">
                    <type>DxEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Constipation+from+inadequate+dietary+fiber</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">It+is+possible+that+inadequate+dietary+fiber+is+contributing+to+this+symptom.+Many+people+in+the+U.S.+get+inadequate+fiber+in+their+diet.++However%2c+there+is+a+more+likely+primary+cause+for+his+constipation.+</value>
                    </values>
                  </panel>
                  <panel id=""77234c49ac"">
                    <type>DxEntry</type>
                    <values>
                      <value id=""DiagnosisValue"">Opioid-Induced+Constipation</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Constipation+is+one+of+the+most+common+side+effects+of+chronic+opioid+use.+The+patient+reported+a+connection+between+when+he+was+on+opioids+and+when+he+had+constipation%2c+so+the+diagnosis+seems+likely.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
      <section id=""_Treatment_PlanningSection"">
        <name>_Treatment_PlanningSection</name>
        <tabs>
          <tab id=""Tx Planning Info"">
            <type>TextboxTab</type>
            <name>Tx+Planning+Info</name>
            <panels>
              <panel id=""cfeae53d50"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3e%3ccolor%3d%23FF0000%3eBACKGROUND+INFORMATION+FOR+OPIOID+RISK+SCREENING%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cB%3eOpioid+Risk+Tool+(ORT)+%3c%2fb%3e+(Webster%2c+2005)%0d%0aQuestions+cover%3a%0d%0a1.+Family+history+of+substance+abuse%0d%0a2.+Personal+history+of+substance+abuse%0d%0a3.+Age+(16-45+has+the+highest+risk)%0d%0a4.+History+of+preadolescent+sexual+abuse+(for+females+only)%0d%0a5.+Psychological+Disease%0d%0aAlternative+screening+survey%3a+Diagnosis%2c+Intractability%2c+Risk%2c+Efficacy+(DIRE)+(Belgrade%2c+2006)%0d%0a%3cb%3ePrescription+Drug+Monitoring%3c%2fb%3e%0aConsult+prescription+drug+monitoring+program+database+before+prescribing+opioids+and+during+treatment%3a+Look+at+total+opioid+doses+and+dangerous+drug+combinations.+Check+database+at+least+every+3+months+and+consider+checking+at+every+prescription.%0d%0a%0aRecommendations+from%3a+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016+(Dowell%2c+et+al.%2c+2016)</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Assess Opioid Risk"">
            <type>Screen_AssessTab</type>
            <name>Assess+Opioid+Risk</name>
            <panels>
              <panel id=""250b9a8c23"">
                <type>ScreenAssessEntryPanel</type>
                <values>
                  <value id=""SurveyTitleValue"">Opioid+Risk+Tool+(ORT)+(Webster%2c+2005)</value>
                  <value id=""ScoreValue"">11+points+(7+points%2c+if+you+don%27t+count+his+problem+with+taking+prescribed+opioids+too+early)</value>
                  <value id=""ScoreKeyValue"">Low+risk+0%e2%80%933+points%0aModerate+risk+4%e2%80%937+points%0aHigh+risk+%3e7+points</value>
                </values>
                <panels>
                  <panel id=""73e4fd4627"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Family+History+of+Substance+Abuse+of+Alcohol%2fDrugs</value>
                      <value id=""AnswerValue"">Yes+%e2%80%93+Father+alcohol+(score+for+males+4+points)</value>
                    </values>
                  </panel>
                  <panel id=""3f66ce6ea2"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Personal+History+of+Substance+Abuse+%e2%80%93+Alcohol</value>
                      <value id=""AnswerValue"">No</value>
                    </values>
                  </panel>
                  <panel id=""d4fdd8188a"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Personal+History+of+Substance+Abuse+%e2%80%93+Illegal+Drugs+</value>
                      <value id=""AnswerValue"">No</value>
                    </values>
                  </panel>
                  <panel id=""e29e1675f9"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Personal+History+of+Substance+Abuse+%e2%80%93+Prescription+Drugs</value>
                      <value id=""AnswerValue"">No*+However%2c+this+might+be+interpreted+as+a+score+of+5+points%2c+because%2c+although+Mr.+Wright+answers+no%2c+the+answer+is+actually+%22yes%22+according+to+other+history.+</value>
                    </values>
                  </panel>
                  <panel id=""91d77cc9d2"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Age+16-45</value>
                      <value id=""AnswerValue"">Yes+(score+1+point)</value>
                    </values>
                  </panel>
                  <panel id=""071de89724"">
                    <type>SurveyQuestionEntry</type>
                    <values>
                      <value id=""QuestionValue"">Psychological+Disease+%e2%80%93+Attention+Deficit+Hyperactivity+Disorder%2c+Obsessive+Compulsive+Disorder%2c+Bipolar+Disorder%2c+Schizophrenia</value>
                      <value id=""AnswerValue"">Yes%2c+mild+ADHD+(score+1+point)</value>
                    </values>
                  </panel>
                </panels>
                <pins>
                  <quiz>
                    <questions>
                      <panel id=""9c2be27d54"">
                        <type>QuizQuestion</type>
                        <values>
                          <value id=""QuestionValue"">Please+interpret+Mr.+Wright%27s+Opioid+Risk+Tool+Score.+</value>
                          <value id=""OptionTypeValue"">Multiple+Choice</value>
                        </values>
                        <panels>
                          <panel id=""94c0152c13"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Low</value>
                              <value id=""OptionTypeValue"">Incorrect</value>
                              <value id=""FeedbackValue"">Mr.+Wright+scored+between+7+and+11+points+on+the+Opioid+Risk+Tool.+Low+risk+is+0-3+points.</value>
                            </values>
                          </panel>
                          <panel id=""e55f903fb8"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Moderate</value>
                              <value id=""OptionTypeValue"">Incorrect</value>
                              <value id=""FeedbackValue"">Mr.+Wright+scored+between+7+and+11+points+on+the+Opioid+Risk+Tool.+Moderate+risk+scores+are+4+to+7+points.+</value>
                            </values>
                          </panel>
                          <panel id=""464bd55c2c"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">High</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">Mr.+Wright+scored+between+7+and+11+points+on+the+Opioid+Risk+Tool.+High+Risk+scores+are+7+or+more+points.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                    </questions>
                  </quiz>
                </pins>
              </panel>
            </panels>
          </tab>
          <tab id=""Consultation"">
            <type>ConsultationTab</type>
            <name>Consultation</name>
            <panels>
              <panel id=""02d33aeeeb"">
                <type>ConsultationEntryPanel</type>
                <values>
                  <value id=""ReasonValue"">Mr.+Wright+has+had+some+aberrant+drug-related+behavior+that+may+be+due+to+addiction+or+chronic+inadequately+relieved+pain.+Please+advise+on+the+possibility+of+a+taper+and+the+best+approach+if+you+agree+that%27s+a+good+choice.</value>
                  <value id=""OptionsValue"">Consultation+Options%3a</value>
                </values>
                <panels>
                  <panel id=""a3cab3c482"">
                    <type>ConsultationOptionEntry</type>
                    <values>
                      <value id=""ConsultantValue"">Pain+%26+Addiction+Specialist+</value>
                      <value id=""ResponseValue"">First%2c+consider+that+his+behavior+could+be+the+result+of+poorly+managed+pain+rather+than+other+reasons+people+misuse+opioids.+I%27d+make+sure+he+has+maximal+support+pertaining+to+his+opioid+medications.+Support+should+include+a+highly+structured+treatment+with+frequent+follow-up+visits+monitoring+all+signs+of+addiction+including+urine+and+drug+tests%2c+prescription+drug+monitoring%2c+etc.+You%27ll+need+to+start+to+diversify+his+pain+management+first%2c+spreading+it+among+multiple+disciplines+and+interventions+before+you+reduce+to+opioids.+The+important+thing+is+to+keep+the+pain+managed+while+you+taper.+His+response+to+this+plan%2c+whether+he+can+tolerate+it%2c+will+help+us+understand+whether+there+is+a+need+for+addiction+treatment+as+well.+</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Obtaining+a+consultation+is+appropriate+if+the+case+is+beyond+your+level+of+experience+or+training.+The+specialist+should+be+able+to+help+you+determine+whether+a+referral+is+indicated+or+whether+you+can+manage+the+case+with+some+guidance.+Unfortunately%2c+few+addiction+specialists+also+specialize+in+the+management+of+pain.+</value>
                    </values>
                  </panel>
                  <panel id=""f49ab378e3"">
                    <type>ConsultationOptionEntry</type>
                    <values>
                      <value id=""ConsultantValue"">Substance+use+counselor</value>
                      <value id=""ResponseValue"">First%2c+consider+that+he+may+have+had+poorly+managed+pain+rather+than+the+other+reasons+people+misuse+opioids.+Then+I%27d+make+sure+he+has+maximal+support+pertaining+to+his+opioid+medications+you+know%2c+a+highly+structured+treatment+with+frequent+follow-up+visits+monitoring+all+signs+of+addiction+including+urine+and+drug+tests%2c+prescription+drug+monitoring%2c+etc.+You%27ll+need+to+start+to+diversify+his+pain+management+first%2c+spreading+it+among+multiple+disciplines+and+interventions+before+you+reduce+to+opioids.+The+important+thing+is+to+keep+the+pain+managed+while+you+taper.+His+response+to+this+plan%2c+whether+he+can+tolerate+it%2c+will+help+us+understand+whether+there+is+a+need+for+addiction+treatment+as+well.+</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Obtaining+a+consultation+is+appropriate+if+the+case+is+beyond+your+level+of+experience+or+training.+The+specialist+should+be+able+to+help+you+determine+whether+a+referral+is+indicated+or+whether+you+can+manage+the+case+with+some+guidance.%0a%0aYou+are+more+likely+to+find+this+specialist+in+areas+that+don%e2%80%99t+have+major+medical+centers%2c+than+one+with+expertise+in+both+addiction+and+pain%2c+which+would+be+even+better.+</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
          <tab id=""PreTreatment Dialogue"">
            <type>TextboxTab</type>
            <name>PreTreatment+Dialogue</name>
            <panels>
              <panel id=""391b1ef9a5"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3eInstructions%3c%2fb%3e%3a+Make+selections+for+what+the+provider+should+say+in+a+discussion+of+potential+treatments.+</value>
                </values>
                <pins>
                  <dialogue>
                    <conversation>
                      <panel id=""4d659851ae"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">I+haven%27t+done+any+more+physical+therapy+because+it%27s+painful.+</value>
                        </values>
                      </panel>
                      <panel id=""1ab71619aa"">
                        <type>DialogueChoiceEntry</type>
                        <values>
                          <value id=""QuestionValue"">Choose+the+best+response+from+the+provider+regarding+physical+therapy%3a</value>
                        </values>
                        <panels>
                          <panel id=""aa8d8d0fcc"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Physical+therapy+is+designed+to+make+you+push+your+limits%2c+which+does+cause+some+discomfort.</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">It+is+important+to+be+open+and+honest+with+patients+about+the+challenges+of+recommended+treatment%2c+so+this+is+a+good+choice.+It+would+also+be+important+to+tell+him+about+the+benefits+of+physical+therapy+and+tips+to+make+it+less+painful.+</value>
                            </values>
                          </panel>
                          <panel id=""749339898f"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">OK.+We%27ll+focus+on+other+treatments+since+physical+therapy+has+already+been+tried.</value>
                              <value id=""OptionTypeValue"">Incorrect</value>
                              <value id=""FeedbackValue"">Just+because+physical+therapy+has+been+tried+before+doesn%27t+necessarily+mean+it+will+not+help.++</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                      <panel id=""40617f3fec"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Physical+therapy+is+designed+to+make+you+push+your+limits+which+does+cause+some+discomfort.+Pushing+a+little+is+likely+to+make+you+stronger+and+function+better.+</value>
                        </values>
                      </panel>
                      <panel id=""038430274c"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">Yes%2c+I+suppose+you+are+right.+</value>
                        </values>
                      </panel>
                      <panel id=""fb96be8a34"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Some+people+take+a+dose+of+pain+medication+about+an+hour+before+physical+therapy+to+make+it+more+comfortable+and+productive.+</value>
                        </values>
                      </panel>
                      <panel id=""0c0c7a5c3c"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I+hadn%e2%80%99t+thought+of+that.+That+makes+sense.+</value>
                        </values>
                      </panel>
                      <panel id=""722753f8e2"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I+want+to+work+with+you+to+get+you+the+best+pain+management+possible.+It+will+involve+medications+and+some+other+types+of+treatment.+It+is+important+for+you+to+use+all+of+the+recommended+treatments%2c+because+they+add+up.+They+work+together+to+provide+the+best+possible%2c+safe+pain+management.+</value>
                        </values>
                      </panel>
                      <panel id=""dd43ef242d"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">That+sounds+good.+I+hope+it+makes+a+difference.+</value>
                        </values>
                      </panel>
                      <panel id=""6187798542"">
                        <type>DialogueChoiceEntry</type>
                        <values>
                          <value id=""QuestionValue"">Should+you+bring+up+taking+his+medications+according+to+the+prescribed+schedule+again+or+should+you+avoid+embarrassing+him+by+bringing+it+up+again+since+this+discussion+covers+it+well+enough%3f+</value>
                        </values>
                        <panels>
                          <panel id=""81e08474b6"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Avoid+making+the+patient+feel+bad+by+bringing+it+up+again+since+this+discussion+covers+it+well+enough.</value>
                              <value id=""OptionTypeValue"">Incorrect</value>
                              <value id=""FeedbackValue"">It+is+important+to+obtain+an+explicit+understanding+with+Mr.+Wright+that+you+expect+him+to+follow+the+instructions+for+taking+his+medication+and+to+explain+why+this+is+important.+</value>
                            </values>
                          </panel>
                          <panel id=""240ff55cf8"">
                            <type>QuizQuestionOption</type>
                            <values>
                              <value id=""OptionValue"">Bring+up+taking+his+medications+according+to+the+prescribed+schedule.</value>
                              <value id=""OptionTypeValue"">Correct</value>
                              <value id=""FeedbackValue"">Important+instructions+can+be+delivered+in+a+firm%2c+caring+tone%2c+without+shaming+the+patient.+It+is+important+to+obtain+an+explicit+understanding+with+Mr.+Wright+that+you+expect+him+to+follow+the+instructions+for+taking+his+medication+and+to+explain+why+this+is+important.+</value>
                            </values>
                          </panel>
                        </panels>
                      </panel>
                      <panel id=""9bd1b52837"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">I+need+you+to+promise+you%27ll+stick+with+the+program+we+agree+upon%2c+including+taking+your+medications+according+to+the+schedule%2c+in+order+for+this+program+to+work+and+work+safely.</value>
                        </values>
                      </panel>
                      <panel id=""2362aa2436"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Patient</value>
                          <value id=""charColor"">RGBA(0.106%2c+0.722%2c+0.059%2c+1.000)</value>
                          <value id=""dialogueText"">That+makes+sense.+OK.+I%27ll+agree+to+that.</value>
                        </values>
                      </panel>
                      <panel id=""fe365e68bd"">
                        <type>DialogueEntryTest2</type>
                        <values>
                          <value id=""characterName"">Provider</value>
                          <value id=""charColor"">RGBA(0.000%2c+0.251%2c+0.957%2c+1.000)</value>
                          <value id=""dialogueText"">Okay.+Let%27s+talk+about+the+non-medication+treatments+first.+</value>
                        </values>
                      </panel>
                    </conversation>
                  </dialogue>
                </pins>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
      <section id=""_TreatmentSection"">
        <name>_TreatmentSection</name>
        <tabs>
          <tab id=""Non-Pharm"">
            <type>TreatmentTab</type>
            <name>Non-Pharm</name>
            <panels>
              <panel id=""a6b0ae1cb4"">
                <type>TreatmentOtherEntryPanel</type>
                <values>
                  <value id=""TreatmentValue"">Physical+therapy</value>
                  <value id=""ResponseValue"">Motivating+Mr.+Wright+to+stay+with+his+physical+therapy+was+initially+challenging%2c+but+after+several+sessions%2c+Mr.+Wright+responded+well+to+the+palliative+and+strengthening+treatments.++</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Further+patient+education+regarding+physical+therapy+could+focus+on+its+benefits+of+pain+relief+and+strength+building+as+well+as+neuromuscular+education.+A+customized+exercise+plan+could+be+developed.++Even+if+PT+does+not+improve+Mr.+Wright%27s+pain%2c+an+important+benefit+of+physical+therapy+is+to+help+avoid+further+deterioration.+%0a%0aFurthermore%2c+a+multidisciplinary+pain+management+approach+is+important%2c+to+minimize+the+use+of+opioids+as+much+as+possible+given+his+high+level+of+risk.+</value>
                </values>
              </panel>
              <panel id=""69f777c09d"">
                <type>TreatmentOtherEntryPanel</type>
                <values>
                  <value id=""TreatmentValue"">Counseling</value>
                  <value id=""ResponseValue"">Mr.+Wright+gained+pain+coping+skills+slowly+and+was+able+to+stop+using+alcohol+to+avoid+drug+interactions.+His+depression+also+improved.+</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Because+Mr.+Wright+is+in+a+high-risk+group+for+opioid+use+disorder+or+misuse%2c+a+multidisciplinary+treatment+plan+for+pain+management+that+helps+minimize+that+risk+is+important%2c+including+behavioral+supports.+Some+of+his+dangerous%2c+aberrant+drug-related+behavior+seems+to+be+motivated+by+unmanaged+pain+so+developing+coping+skills+learning+how+to+avoid+misuse+is+important.+%0a%0aCognitive+behavioral+therapy+has+been+used+effectively+to+help+in+coping+with+pain.+Behavioral+therapy+could+also+be+used+to+help+him+avoid+cues+to+misuse+of+opioids.</value>
                </values>
              </panel>
              <panel id=""98400e35d7"">
                <type>TreatmentOtherEntryPanel</type>
                <values>
                  <value id=""TreatmentValue"">Weight+loss</value>
                  <value id=""ResponseValue"">Mr.+Wright%27s+osteoarthritis+progressed+less+rapidly+than+it+would+have+otherwise+and+he+noticed+a+little+better+functioning+and+less+pain+after+getting+his+BMI+down+to+25.+</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Because+Mr.+Wright+has+a+BMI+of+27.5%2c+which+is+overweight%2c+he+should+be+advised+to+lose+weight+until+he+is+at+a+weight+that+would+have+his+BMI+is+under+25.+The+American+Association+of+Osteopathic+Surgeons+guidelines+for+treatment+of+knee+osteoarthritis+(AAOS%2c+2011)+recommend+weight+loss+if+BMI+is+over+25.+</value>
                </values>
              </panel>
              <panel id=""e2ea62fcc6"">
                <type>NonPharma</type>
                <values>
                  <value id=""TreatmentValue"">Self-managed+low+impact+exercise</value>
                  <value id=""ResponseValue"">Mr.+Wright+was+not+able+to+complete+these+exercises+at+first%2c+but+after+two+weeks+of+physical+therapy%2c+he+started+doing+them+under+the+supervision+of+the+physical+therapist.+After+a+month%2c+he+was+directed+to+continue+them+indefinitely+on+his+own.+Eventually%2c+after+two+months%2c+he+remarked+that+they+make+a+noticeable+improvement+in+his+pain+and+functioning.+</value>
                  <value id=""OptionTypeValue"">Partially+Correct</value>
                  <value id=""FeedbackValue"">Exercise+is+very+important+in+the+long+term+plan%2c+but+Mr.+Wright+may+not+be+able+to+engage+in+it+until+after+his+pain+is+a+little+better+managed.+The+American+Association+of+Osteopathic+Surgeons+guidelines+for+treatment+of+knee+osteoarthritis+(AAOS%2c+2011)+recommends+self-managed+low-impact+exercise.</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Pain Med Info"">
            <type>TextboxTab</type>
            <name>Pain+Med+Info</name>
            <panels>
              <panel id=""8614568934"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3e%3ccolor%3d%23886382%3eBACKGROUND+INFORMATION+FOR+PHARMACOLOGICAL+TREATMENT%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cB%3ePrescribing+Guidelines+for+Chronic+Opioid+Therapy%3c%2fb%3e%0a%0d%0a+%e2%80%a2+Use+other+treatments+first+if+possible%3a+Non-opioid+pharmacologic+medication+and+nonpharmacologic+therapy+are+preferred+treatment+for+chronic+pain.+Only+consider+opioids+if+benefits+for+both+pain+and+functioning+are+likely+to+outweigh+risks.+If+opioids+are+prescribed%2c+minimize+their+use+by+combining+with+non-opioids+and+non-pharmacological+therapy.+%0d%0a%0d%0a+%e2%80%a2+Use+treatment+goals%3a+Set+realistic+treatment+goals+for+pain+and+function+at+the+outset.+Explain+that+treatment+will+continue+only+if+the+risk+vs.+benefit+ratio+is+favorable+with+%22clinically+meaningful+improvement.%22%0d%0a%0d%0a+%e2%80%a2+Discuss+known+risks+and+realistic+benefits+of+opioid+therapy+with+the+patient+before+starting.+Define+patient+and+clinician+responsibilities+for+managing+therapy.+%0d%0a%0d%0a+%e2%80%a2+Use+immediate-release%2c+not+extended-release%2flong-acting+opioids+(ER%2fLAs)+when+starting+opioid+therapy+for+chronic+pain.+Note+that+the+FDA%27s+REMS+(Risk+Evaluation+and+Mitigation+Strategy)++requires+that+the+pharmaceutical+companies+provide+training+for+opioid+prescribers+and+special+training+for+ER%2fLA+opioids.+%0a%0d+%0a+%e2%80%a2+Use+lowest+possible+dose%3a+Reassess+benefits+vs.+risks+carefully+when+considering+a+dosage+increase+to+%e2%89%a550+morphine+milligram+equivalents+(MME)%2fday.+Avoid+increasing+the+dose+to+%e2%89%a590+MME%2fday+or+carefully+justify.+%0d%0a%0d%0a+%e2%80%a2+Avoid+prescribing+opioids+together+with+benzodiazepines.+%0d%0a%0d%0a+%e2%80%a2+Treat+opioid+use+disorder%3a+Treat+or+arrange+treatment+for+opioid+use+disorder%2c+usually+with+medication-assisted+treatment%2c+i.e.%2c+buprenorphine+or+methadone%2c+in+combination+with+behavioral+therapy.+%0d%0a%0d%0a+%e2%80%a2+Evaluation+of+benefits+vs.+risks+is+ongoing%3a+Evaluate+benefits+and+risks+with+patients+within+1+to+4+weeks+of+starting+chronic+opioid+therapy+or+a+dose+increase.+Reevaluate+at+least+every+3+months.+Taper+to+a+lower+dosage+or+discontinue+opioids+if+benefits+do+not+exceed+risks.+%0d%0a%0d%0a%3cb%3eReference%3a%3c%2fb%3e+%0aRecommendations+from%3a+%3ci%3eCDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016%3c%2fi%3e+(Dowell%2c+et+al.%2c+2016)%0d.%0a%0a%3cb%3eUse+of+Treatment+Agreements+When+Prescribing+Chronic+Opioid+Therapy%3c%2fb%3e%0a%0aGuidelines+for+treating+chronic+pain+with+opioids+(CDC+-+Dowell+et+al.%2c+2016)+recommend+the+use+of+treatment+agreements+when+prescribing+chronic+opioids%3a%0d%0a%0d%0a+%e2%80%a2+Patient%2fprovider+treatment+agreements%3a+Consider+use+of+written+agreements+that+describe+responsibilities+of+both+the+patient+and+prescribing+provider+and+the+treatment+structure+that+helps+prevent+addiction%2c+misuse%2c+and+diversion.+Include+patient+education+on+using+as+directed%2c+safe+storage%2c+keeping+appointments%2c+not+sharing+medications%2c+etc.%0d%0a%0d%0a+%e2%80%a2+Increase+treatment+structure+for+higher+risk+patients%3a+The+treatment+agreement+should+describe+additional+requirements+for+patients+with+high+risk.+For+example%2c+more+frequent+appointments+and+urine+drug+testing+might+be+required+with+higher+risk.%0d%0a%0d%0a+%e2%80%a2+Plan+for+stopping+opioid+treatment+before+starting%3a+Describe+a+plan+in+the+treatment+agreement+that+includes+the+conditions+under+which+treatment+will+be+stopped%2c+and+a+plan+for+tapering+and+providing+psychosocial+supports+when+stopping.%0d%0a%0d%0a%0d%0a%3cb%3eTherapeutic+Injection+for+Knee+Pain%3c%2fb%3e%0a%0aHyaluronic+acid+or+hyaluronan+injections+are+used+to+introduce+a+lubricating+fluid+into+the+joint.+This+treatment+is+often+used+in+knee+osteoarthritis.+%0d%0a%0a%0a%3cb%3eDefinitions%3a+%0d%3c%2fb%3e%0a%0aA+%22rescue+medication%22+in+pain+management+is+an+analgesic+medication+for+when+the+patient+still+has+pain+after+taking+regular+pain+medications.+%0d%0a%0d%0a%22Adjuvant%22+pain+medications+are+added+to+analgesics+to+improve+the+amount+of+pain+management.+Antidepressants+and+anti-epileptics+are+most+commonly+used.</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Rx"">
            <type>TreatmentTab</type>
            <name>Rx</name>
            <panels>
              <panel id=""99314b962a"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">Prescribe+current+opioid+but+start+a+slow+taper</value>
                  <value id=""ResponseValue"">Mr.+Wright+was+able+to+tolerate+a+very+slow+taper+of+his+opioid+dose.+The+tapering+process+included+adding+non-opioid+analgesics+and+adjuvants+as+well+as+non-pharmacological+treatments+in+a+comprehensive+multidisciplinary+treatment+plan.+With+this+approach%2c+he+was+better+able+to+stick+to+his+dosing+schedule+and+eventually+felt+more+in+control+and+less+depressed</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Currently%2c+Mr.+Wright+has+insufficient+pain+management%2c+but+this+should+be+addressed+via+the+addition+of+non-opioid+analgesics+and+adjuvant+medications+plus+the+addition+of+non-pharmacological+therapies+in+a+comprehensive+multidisciplinary+pain+management+plan.+Tapering+can+sometimes+be+accomplished+slowly+without+any+decrease+in+pain+relief.+%0a%0aNote+that+the+FDA+requires+manufacturers+of+extended-release+opioids+to+train+providers+in+their+safe+and+appropriate+use.+%0d</value>
                </values>
              </panel>
              <panel id=""bec0ae3e50"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">Medication-assisted+treatment+for+opioid+use+disorder</value>
                  <value id=""ResponseValue"">Mr.+Wright+may+not+need+treatment+for+opioid+use+disorder.+</value>
                  <value id=""OptionTypeValue"">Partially+Correct</value>
                  <value id=""FeedbackValue"">Not+the+best+choice+at+this+time%2c+but+may+be+needed+if+other+treatments+fail.+If+that+were+to+happen%2c+medication-assisted+treatment+for+opioid+use+disorder+(e.g.%2c+buprenorphine+or+methadone)+could+be+considered+along+with+providing+non-opioid+pain+relief.+Patients+often+experience+some+pain+relief+from+buprenorphine+itself+while+it+is+being+used+to+treat+opioid+use+disorder.</value>
                </values>
              </panel>
              <panel id=""f778d17617"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">Add+immediate-release+oxycodone+for+breakthrough+pain</value>
                  <value id=""ResponseValue"">If+Mr.+Wright+is+prescribed+the+immediate+release+oxycodone+he+would+be+more+likely+to+overdose+or+develop+opioid+use+disorder.+</value>
                  <value id=""OptionTypeValue"">Incorrect</value>
                  <value id=""FeedbackValue"">When+a+patient+experiences+breakthrough+pain+despite+taking+opioids%2c+it+does+not+necessarily+mean+that+more+opioids+are+needed.+An+NSAID+or+other+non-opioid+analgesic+should+be+added+instead.+A+multidisciplinary+approach+to+pain+management%2c+including+non-pharmacological+treatments%2c+is+also+needed.+%0a%0aWith+the+unsafe+behavior+of+taking+his+medication+early%2c+prescribing+immediate+release+oxycodone+to+take+as+needed+would+probably+be+too+dangerous%2c+with+risk+of+overdose+and+addiction.+%0d</value>
                </values>
              </panel>
              <panel id=""b02056dbe9"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">Naloxone+overdose+kit</value>
                  <value id=""ResponseValue"">Mr.+Wright+never+needed+to+use+the+naloxone+overdose+kit+but+was+glad+to+have+it.+He+told+people+who+are+close+to+him+about+the+kit+and+how+to+use+it+if+he+should+be+found+non-responsive+from+an+apparent+overdose.+</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Current+guidelines+for+managing+chronic+pain+recommend+offering+to+prescribe+a+naloxone+kit+to+treat+overdose+when+chronic+opioids+are+prescribed+(Dowell+et+al.%2c+2016).+</value>
                </values>
              </panel>
              <panel id=""a31f47592d"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">Increase+dose+of+long-acting+opioid</value>
                  <value id=""ResponseValue"">Mr.+Wright%27s+pain+was+managed+well+by+the+increased+dose+initially%2c+but+soon+he+was+reporting+inadequate+pain+relief+and+breakthrough+pain+again.+Furthermore%2c+his+more+addictive+behavior+increased.+</value>
                  <value id=""OptionTypeValue"">Incorrect</value>
                  <value id=""FeedbackValue"">Mr.+Wright+has+moderate+to+high+risk+for+taking+opioids+and+opioids+are+not+the+first-line+recommended+treatment+for+his+condition.+A+taper+off+opioids+if+possible%2c+plus+a+multidisciplinary+approach+to+pain+management%2c+including+non-opioid+analgesics+and+non-pharmacological+treatments%2c+is+needed+instead.+%0a%0aNote+that+the+FDA+requires+manufacturers+of+extended-release+opioids+to+train+providers+in+their+safe+and+appropriate+use.+%0d</value>
                </values>
              </panel>
              <panel id=""4a965e81c6"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">NSAID+analgesic</value>
                  <value id=""ResponseValue"">Mr.+Wright+was+surprised+to+find+that+an+over-the-counter%2c+easily+absorbed+ibuprofen%2c+taken+only+when+his+pain+flared+up%2c+provided+significant+pain+relief.</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Non-opioid+analgesics+should+be+used+to+spare+the+dose+of+opioids+that+is+needed+for+pain+management+(Dowell+et+al.%2c+2016).++Mr.+Wright+had+stopped+using+NSAIDs+and+acetaminophen+because+they+did+not+provide+sufficient+pain+relief+by+themselves.+He+needs+to+be+taught+that+NSAIDs+can+be+a+part+of+a+comprehensive+treatment+plan+and+that+all+components+of+that+plan+act+together+to+manage+his+pain+safely.+</value>
                </values>
              </panel>
              <panel id=""371680bdcb"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">Docusate+sodium+stool+softener</value>
                  <value id=""ResponseValue"">Mr.+Wright+continued+to+have+no+problems+from+his+opioid-induced+constipation.</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Docusate+stool+softener+is+an+over-the-counter+medication+that+is+one+of+the+first-choice+medications+when+treating+opioid-induced+constipation+(Kumar+et+al.%2c+2014).+It+should+be+prescribed+as+long+as+Mr.+Wright+continues+taking+opioids.+He%27s+had+constipation+before+from+taking+opioids%2c+so+it+is+likely+to+continue.+The+constipation+was+managed+previously+with+this+stool+softener%2c+so+this+is+a+good+place+to+start+with+his+treatment.+It+avoids+potential+drug+interactions+with+certain+laxatives.+Bulk+laxatives+containing+psyllium+are+not+recommended+due+to+risk+of+bowel+impaction.+</value>
                </values>
              </panel>
              <panel id=""7b4451aa06"">
                <type>TreatmentPharmaEntryPanel</type>
                <values>
                  <value id=""MedicationValue"">Continue+antidepressant</value>
                  <value id=""ResponseValue"">Mr.+Wright+said+he+thought+that+his+pain+was+better+managed+with+his+antidepressant+and+he+continued+to+have+no+significant+side+effects.+His+depression+continued+to+be+minimal.+</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Managing+depression+is+an+important+part+of+chronic+pain+management.+Antidepressants+also+sometimes+help+with+chronic+pain+as+adjuvant+treatment%2c+especially+neurological+pain.+</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Surg"">
            <type>TreatmentTab</type>
            <name>Surg</name>
            <panels>
              <panel id=""e99e01c99e"">
                <type>TreatmentOtherEntryPanel</type>
                <values>
                  <value id=""TreatmentValue"">Surgical+consultation</value>
                  <value id=""ResponseValue"">At+this+consultation%2c+Chad+learned+that+arthroplasty+is+likely+to+help+in+his+case.+He+is+not+interested+at+this+time+due+to+cost+and+time+factors.+He+asks+the+surgeon+about+alternatives+to+more+surgery+and+learns+that+a+hyaluronic+acid+injection+acts+as+a+joint+lubricant+and+may+help+his+knee%2c+but+the+effectiveness+is+uncertain+(AAOS%2c+2011).+He+makes+an+appointment+to+get+hyaluronic+acid+injections+because+they+have+helped+in+the+past.++</value>
                  <value id=""OptionTypeValue"">Correct</value>
                  <value id=""FeedbackValue"">Referral+to+a+surgeon+is+indicated+because+Mr.+Wright+has+not+had+a+surgical+evaluation+recently.+There+may+be+newer+techniques+or+materials+to+increase+the+range+of+options+since+the+last+time+he+saw+a+surgeon.++For+example%2c+he+might+benefit+from+therapeutic+injections+(e.g.%2c+hyaluronic+acid)+or+joint+replacement+although+the+evidence+is+mixed+for+their+effectiveness+in+osteoarthritis+(AAOS%2c+2011).+He+also+might+benefit+from+arthroplasty+(knee+replacement+surgery).+</value>
                </values>
              </panel>
              <panel id=""73970d41dd"">
                <type>TreatmentOtherEntryPanel</type>
                <values>
                  <value id=""TreatmentValue"">No+surgical+consultation+needed</value>
                  <value id=""ResponseValue"">No+results</value>
                  <value id=""OptionTypeValue"">Incorrect</value>
                  <value id=""FeedbackValue"">Referral+to+a+surgeon+is+indicated+because+Mr.+Wright+hasn%27t+had+a+surgical+evaluation+recently.+There+may+be+newer+techniques+or+materials+to+increase+the+range+of+options+since+the+last+time+he+saw+a+surgeon+that+he+should+be+aware+of+for+planning+purposes.++For+example%2c+he+might+benefit+from+therapeutic+injections+(e.g.%2c+hyaluronic+acid)+or+knee+replacement+surgery+(arthroplasty).</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Tx Quiz"">
            <type>QuizTab</type>
            <name>Tx+Quiz</name>
            <panels>
              <panel id=""075cf22f4b"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">When+evaluating+patients+for+potential+chronic+opioid+therapy%2c+physicians+should+triage+patients+to+stratify+risk.+%0a%0aChoose+which+of+the+following+describes+a+patient+having+moderate-risk+for+opioid+overdose%2c+diversion%2c+and+addiction%3a++(Choose+all+that+apply)</value>
                  <value id=""OptionTypeValue"">Checkboxes</value>
                </values>
                <panels>
                  <panel id=""46e47af59d"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Current+substance+use+problems</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Having+current+substance+use+problems+would+put+the+patient+in+a+severe+risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+does+not+have+a+substance+use+problem+other+than+his+misuse+of+opioids+that+were+prescribed+for+him.+</value>
                    </values>
                  </panel>
                  <panel id=""c6779cb8b1"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Comorbid+minor+or+past+major+mental+health+problem</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">A+comorbid+minor+or+past+major+mental+health+problem+would+put+the+patient+in+a+severe-risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+has+depression+that+is+fairly+well-managed+with+an+anti-depressant.+If+this+were+his+only+problem%2c+he+would+be+considered+to+have+moderate+risk.+</value>
                    </values>
                  </panel>
                  <panel id=""cf6a6636b8"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Family+history+of+substance+abuse</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Family+history+of+substance+use+would+put+the+patient+in+a+severe-risk+category%2c+not+moderate.%0a%0aMr.+Wright%27s+family+did+not+have+a+history+of+substance+abuse.+</value>
                    </values>
                  </panel>
                  <panel id=""144e94c7e1"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Major+untreated+mental+health+problems</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Major+untreated+mental+health+problems+would+put+the+patient+in+a+severe-risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+has+depression+that+is+fairly+well-managed+with+an+anti-depressant%2c+which+is+not+a+major+mental+health+problem.+</value>
                    </values>
                  </panel>
                  <panel id=""d84b5c7569"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Changing+opioid+medication+dosage+without+consulting+providor</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Changing+opioid+medication+dosage+without+consulting+providor+puts+the+patient+in+a+severe-risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+changed+the+dose+of+his+opioid+medication+without+consulting+a+provider+an+so+should+be+considered+high-risk.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
              <panel id=""9eb736f631"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">To+minimize+risk+for+addiction%2c+the+best+strategy%2c+if+opioids+are+clearly+needed%2c+for+starting+a+patient+on+chronic+opioid+therapy+is%3a++(Choose+the+best+answer)%0d</value>
                  <value id=""OptionTypeValue"">Multiple+Choice</value>
                </values>
                <panels>
                  <panel id=""5c2c925130"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Use+long+acting%2fextended+release+opioids+on+a+schedule+for+pain</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</value>
                    </values>
                  </panel>
                  <panel id=""b6f3017ea5"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Use+long+acting%2fextended+release+opioids+as+needed+for+pain</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</value>
                    </values>
                  </panel>
                  <panel id=""e854735798"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Use+immediate+release+opioids+as+needed+for+pain</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+or+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</value>
                    </values>
                  </panel>
                  <panel id=""5a607a03a4"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Use+immediate+release+opioids+on+a+schedule+for+pain</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+or+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
          <tab id=""Pt Ed"">
            <type>QuizTab</type>
            <name>Pt+Ed</name>
            <panels>
              <panel id=""f334537ace"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">Which+of+the+following+patient+education+topics+is+important+currently%3f</value>
                  <value id=""OptionTypeValue"">Checkboxes</value>
                </values>
                <panels>
                  <panel id=""3856122041"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Patient+information+flier+regarding+taking+opioids+safely</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Mr.+Wright+should+be+given+instructions+for+safe+use+of+opioids+while+he+is+still+taking+them.+This+should+include%3a+%0a+%e2%80%a2+What+not+to+take+with+opioids+(benzodiazepines%2c+alcohol%2c+and+sedative-hypnotic+substances)%0a+%e2%80%a2+What+to+do+in+the+event+of+an+overdose+(contact+EMS+and+administer+naloxone)%0a+%e2%80%a2+Safe+storage+and+disposal%0a+%e2%80%a2+Other+safeguards+against+diversion.+%0a%0aMany+practices+also+send+a+follow-up+message%2c+such+as+%22Notes+from+Your+Recent+Visit+to+(Name+of+Practice)%22+that+repeat+any+instructions+that+were+given.+</value>
                    </values>
                  </panel>
                  <panel id=""c60b6eeb5f"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Information+on+opioids+and+constipation</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">This+is+important+information+when+starting+chronic+opioid+therapy+or+if+constipation+develops+as+a+symptom+later.+However%2c+Mr.+Wright+has+been+treated+successfully+for+his+opioid-induced+constipation+using+docusate+stool+softener+for+many+years+and+so+probably+does+not+need+this+information.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
              <panel id=""c15e7ae4e8"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">Which+of+the+following+is+best+to+help+Mr.+Wright+adhere+to+his+treatment+if+opioids+are+prescribed%3f+(Choose+the+best+answer)</value>
                  <value id=""OptionTypeValue"">Multiple+Choice</value>
                </values>
                <panels>
                  <panel id=""b7915ff70e"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Written+and+signed+provider-patient+treatment+agreement</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Written+provider-patient+treatment+agreements+are+recommended+by+the+CDC%27s+guidelines+for+chronic+pain+management+with+opioids+(Dowell+et+al.%2c+2016).+These+agreements+spell+out+your+office+policies+regarding+chronic+opioid+therapy+and+any+additional+rules+or+regulations+needed+for+a+specific+patient.+Treatment+agreements+typically+describe+policies+for%3a%0a+%e2%80%a2+missed+appointments%0a+%e2%80%a2+lost+medications%0a+%e2%80%a2+early+refill+requests%0a+%e2%80%a2+urine+drug+tests%0a+%e2%80%a2+when+treatment+would+be+discontinued%0a+%e2%80%a2+a+plan+for+ending+treatment+%0a%0aA+patient+with+high+risk+might+additionally+be+required+to+bring+in+their+prescription+bottle+for+you+to+reconcile+the+number+of+pills+in+it+with+the+number+expected.+</value>
                    </values>
                  </panel>
                  <panel id=""384c5fef0f"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Treatment+contract</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Contracts+are+no+longer+recommended+for+these+agreements.+The+consensus+in+the+medical+profession+is+that+forming+a+contract+is+not+appropriate.+It+does+not+fit+within+the+patient-centered+care+model+in+which+the+patient+is+empowered+to+take+an+active+partner-like+role+in+his+or+her+treatment.</value>
                    </values>
                  </panel>
                  <panel id=""d5ef0b26ed"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Making+eye+contact+and+asking+for+agreement+with+a+handshake+after+discussing+policies</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">This+would+be+a+good+approach+for+some+patients+but+most+patients+on+chronic+opioid+therapy+need+more+structured+guidelines+and+policies.+The+written+treatment+agreement+is+a+good+compromise+between+a+contract+and+a+handshake.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
          <tab id=""Referral"">
            <type>ReferralTab</type>
            <name>Referral</name>
            <panels>
              <panel id=""102e7beca5"">
                <type>ReferralEntryPanel</type>
                <values>
                  <value id=""ReasonValue"">Mr.+Wright+increased+his+opioid+dose+by+increasing+frequency+without+medical+supervision.+Evaluate+for+Opioid+Use+Disorder</value>
                </values>
                <panels>
                  <panel id=""12a154f55c"">
                    <type>ReferralOptionEntry</type>
                    <values>
                      <value id=""OptionValue"">A+physician+specializing+in+addiction+and+pain</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Recommending+evaluation+for+substance+use+disorder+is+indicated%2c+especially+if+Mr.+Wright+continues+to+modify+the+way+he+takes+his+medication+on+his+own.+An+addiction+specialist+who+specializes+in+pain+would+also+be+ideal.+Unfortunately%2c+there+aren%27t+very+many+individuals+having+these+qualifications.</value>
                    </values>
                  </panel>
                  <panel id=""534edf2c75"">
                    <type>ReferralOptionEntry</type>
                    <values>
                      <value id=""OptionValue"">Substance+abuse+counselor</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">Recommending+evaluation+for+substance+use+disorder+is+indicated%2c+especially+if+Mr.+Wright+continues+to+modify+the+way+he+takes+his+medication+on+his+own.++A+substance+abuse+counselor+could+evaluate+Mr.+Wright+for+opioid+use+disorder.+However%2c+they+typically+do+not+have+much+training+in+pain+management.</value>
                    </values>
                  </panel>
                  <panel id=""00a6701429"">
                    <type>ReferralOptionEntry</type>
                    <values>
                      <value id=""OptionValue"">No+referral+needed.+Opioid+risk+has+been+ruled+out.+</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Recommending+evaluation+for+substance+use+disorder+is+indicated%2c+especially+if+Mr.+Wright+continues+to+modify+the+way+he+takes+his+medication+on+his+own.+</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
          <tab id=""Follow Up"">
            <type>QuizTab</type>
            <name>Follow+Up</name>
            <panels>
              <panel id=""c0b474886d"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">Choose+the+best+follow-up+plan+from+the+following%3a+(Choose+best+answer)</value>
                  <value id=""OptionTypeValue"">Multiple+Choice</value>
                </values>
                <panels>
                  <panel id=""24255c8477"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Follow+up+at+1+week</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Even+without+risk%2c+initial+follow-up+of+around+1-2+weeks+or+sooner+when+there+is+high+risk+indicated+in+chronic+opioid+therapy+to+check+on+pain+control+and+side+effects.+Frequent+follow-up+continues+to+be+important+in+chronic+opioid+therapy+when+the+patient+has+relatively+higher+risk%2c+as+in+this+case+where+he+has+a+history+of+modifying+his+dose+without+medical+approval+and+combining+alcohol+use+with+opioid+use.+</value>
                    </values>
                  </panel>
                  <panel id=""f17f854e42"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Follow-up+optional</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Even+without+risk%2c+initial+follow-up+of+around+1-2+weeks+or+sooner+when+there+is+high+risk+is+indicated+in+chronic+opioid+therapy+to+check+on+pain+control+and+side+effects.+Frequent+follow-up+continues+to+be+important+in+chronic+opioid+therapy+when+the+patient+has+relatively+higher+risk%2c+as+in+this+case+he+has+a+history+of+modifying+his+dose+without+medical+approval+and+combining+alcohol+use+with+opioid+use.+</value>
                    </values>
                  </panel>
                  <panel id=""0892b90894"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Follow-up+not+needed</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Even+without+risk%2c+initial+follow-up+of+around+1-2+weeks+or+sooner+when+there+is+high+risk+is+indicated+in+chronic+opioid+therapy+to+check+on+pain+control+and+side+effects.+Frequent+follow-up+continues+to+be+important+in+chronic+opioid+therapy+when+the+patient+has+relatively+higher+risk%2c+as+in+this+case+he+has+a+history+of+modifying+his+dose+without+medical+approval+and+combining+alcohol+use+with+opioid+use.+</value>
                    </values>
                  </panel>
                </panels>
              </panel>
              <panel id=""654873c9d0"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">How+often+should+the+Prescription+Drug+Monitoring+Plan+(PDMP)+be+examined+as+Mr.+Wright%27s+treatment+continues%3f+(Choose+best+answer)</value>
                  <value id=""OptionTypeValue"">Multiple+Choice</value>
                </values>
                <panels>
                  <panel id=""5b3250ee5e"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Annually</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped+as+well.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</value>
                    </values>
                  </panel>
                  <panel id=""0719c884b9"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Before+initial+prescription+for+opioids</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+also+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</value>
                    </values>
                  </panel>
                  <panel id=""342a4b8002"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Periodically+before+and+during+chronic+opioid+therapy</value>
                      <value id=""OptionTypeValue"">Partially+Correct</value>
                      <value id=""FeedbackValue"">This+is+only+partially+correct%2c+because+PDMP+reports+should+continue+after+opioids+are+tapered.+The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped%2c+as+well.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</value>
                    </values>
                  </panel>
                  <panel id=""94353a0c59"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Before+each+prescription+of+opioids+and+periodically+after+tapering+off+opioids</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped%2c+as+well.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
      <section id=""_SummarySection"">
        <name>_SummarySection</name>
        <tabs>
          <tab id=""Summary"">
            <type>TextboxTab</type>
            <name>Summary</name>
            <panels>
              <panel id=""2f2fcaba0d"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">%3cb%3eCASE+SUMMARY%3c%2fB%3e%0aChad+Wright%2c+34+yo+male.+%0a%0a%3cb%3eHISTORY%3c%2fb%3e%0a%3cb%3eCC%3a%3c%2fb%3e+Requests+opioid+prescription+for+chronic+knee+pain.+%0a%3cb%3eHPI%3a%3c%2fb%3e+Football+injury+to+right+knee+in+2002+with+subsequent+pain.+Treated+surgically+twice+(2002%2c+2008).+Treated+in+recent+years+with+opioids%2flong-acting+oxycodone+for+several+months.+%0a+%e2%80%a2+Pain+is+usually+mild+at+rest+and+moderately+severe+with+extensive+activity+but+fairly+well-controlled+with+opioids.+%0a+%e2%80%a2+History+of+depression+that+he+relates+to+living+with+chronic+pain+and+its+limitations.+Treated+with+antidepressant+Cymbalta%2c+although+he+still+has+mild+symptoms.+%0a+%e2%80%a2+History+of+aberrant+drug-related+behavior+of+increasing+opioid+doses+himself+without+discussing+with+his+prescriber%2c+who+discharged+him+from+the+practice+after+several+early+prescription+renewals.%0a+%e2%80%a2+He+requests+a+prescription+for+long-acting+oxycodone+with+additional+tablets+for+when+his+pain+is+worse.+%0a+%e2%80%a2+Drug+interactions%3a+mild+alcohol+use+with+duloxetine+and+oxycodone.+%0a+%e2%80%a2+Takes+stool+softener+for+constipation.%0a%0a%3cb%3eEVALUATION%3c%2fb%3e%0a%3cb%3eSubstance+Use%3c%2fb%3e%3a%0a+%e2%80%a2+Positive+Substance+Use+Screening+CAGE-AID%3a+Score+3%2b+-+Clinically+significant.+Further+evaluation+for+opioid+use+disorder+is+indicated.+%0a+%e2%80%a2+Alcohol+screening+(AUDIT)%3a+Score+4+-+Below+clinical+significance.+Preventive+education+and+re-screen+in+6+months.%0a+%e2%80%a2+Positive+Opioid+Risk+Tool+(ORT)%3a+Moderate+to+High+risk.+Recommendation+-+Avoid+opioids+and+use+alternatives+such+as+NSAIDs.+If+opioids+are+needed%2c+use+a+high+level+of+treatment+structures+after+warmup.+%0a%3cb%3eUrine+Toxicology%3a+%3c%2fb%3e%0aPositive+only+for+oxycodone+and+oxymorphone%2c+as+expected+due+to+his+recent+use+of+oxycodone.%0a%3cb%3eRadiology%3c%2fb%3e%0aMRI+of+right+knee+shows+patellofemoral+and+tibiofemoral+osteoarthritis%0a%0a%3cb%3eDIAGNOSIS%3c%2fb%3e%0a+%e2%80%a2+Chronic+osteoarthritis%2c+right+knee%0a+%e2%80%a2+Opioid-induced+constipation%0a+%e2%80%a2+Major+Depression+treated+with+antidepressants+but+still+has+mild+depression.+Potential+drug+interaction%3a+duloxetine+and+alcohol+use%0a%0a%3cb%3eTREATMENT+PLAN%3c%2fb%3e%0a+%e2%80%a2+Orthopedic+Surgery+referral+to+evaluate+treatment+options%0aSurgery+Referral+Results%3a+Mr.+Wright+will+try+hyaluronic+acid+injections%2c+but+is+postponing+arthroplasty%2c+which+the+surgeon+recommended.%0a+%e2%80%a2+Counseling+referral+for+residual+depression+and+pain+coping+skills+as+an+alternative+to+drinking+alcohol.%0a+%e2%80%a2+Physical+therapy%0a+%e2%80%a2+Follow-up+plan%3a+Office+visits+every+1-2+weeks+initially%2c+re-evaluate+risk+after+one+month.++</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Learning Points"">
            <type>TextboxTab</type>
            <name>Learning+Points</name>
            <panels>
              <panel id=""066110029a"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">1.+The+diagnosis+of+opioid+use+disorder+involves+a+pattern+of+opioid+use+causing+clinically+significant+impairment+or+distress%2c+and+at+least+two+diagnostic+criteria.+Source+DSM-5+(APA%2c+2013+in+PCSS%2c+2013).%0a%0a2.+Non-opioid+medications+and+nonpharmacological+therapy+are+the+preferred+first-line+treatment+for+chronic+pain+according+to+the+CDC%27s+evidence-based+guidelines+(Dowell+et+al.%2c+2016).+%0a%0a3.+Only+consider+opioids+if%3a%0a++++%e2%80%a2+First-line+treatments+are+not+effective.%0a++++%e2%80%a2+Pain+is+moderate+to+severe.%0a++++%e2%80%a2+Benefits+for+both+pain+and+functioning+are+likely+to+outweigh+risks.%0a%0a4.+If+opioids+are+prescribed%3a%0a++++%e2%80%a2+Prescribing+a+3-day+supply+is+sufficient+for+most+acute+pain%2c+typically+no+more+than+7+days+is+needed.+%0a++++%e2%80%a2+Minimize+dose+by+combining+with+other+therapies+and+multidisciplinary+treatment.%0a++++%e2%80%a2+Use+written+provider-patient+treatment+agreements.%0a++++%e2%80%a2+Monitor+patients+regularly%2c+including+an+assessment+for+continued+need.%0a%0a5.+Prior+to+prescribing+chronic+opioids%3a%0a++++%e2%80%a2+Obtain+a+complete+pain+history%2c+including+all+past+treatments+that+have+been+tried+and+response+to+treatment.%0a++++%e2%80%a2+Consult+your+state%27s+Prescription+Drug+Monitoring+Program+(PDMP).</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""References"">
            <type>TextboxTab</type>
            <name>References</name>
            <panels>
              <panel id=""a0b9bdf5d4"">
                <type>TextboxPanel</type>
                <values>
                  <value id=""CustomContentLabelValue"">Custom+Content%3a</value>
                  <value id=""CustomContentValue"">American+Academy+of+Orthopedic+Surgeons.+Treatment+of+Osteoarthritis+of+the+Knee+2nd+edition.+2011%3b+Available+at%3a+https%3a%2f%2fwww.aaos.org%2fresearch%2fguidelines%2fOAKSummaryofRecommendations.pdf%0a%0aAmerican+Psychiatric+Association.+Diagnostic+and+Statistical+Manual+of+Mental+Disorders%2c+Fifth+Edition.+Washington%2c+DC%2c+American+Psychiatric+Association.+2013+page+541.%0a%0aPCSS-MAT%2fAPA.+Opioid+use+disorder+diagnostic+criteria.+Reprinted+from+Diagnostic+and+Statistical+Manual+of+Mental+Disorders%2c+Fifth+Edition%2c+(Copyright+2013)+American+Psychiatric+Association.+Available+at%3a+http%3a%2f%2fpcssmat.org%2fwp-content%2fuploads%2f2014%2f02%2f5B-DSM-5-Opioid-Use-Disorder-Diagnostic-Criteria.pdf+Accessed+on%3a+2017-04-10.%0a%0d%0aBabor+TF%2c+de+la+Fluente+JF%2c+Saunders+J%2c++Grant+M.+AUDIT%3a+The+Alcohol+Use+Disorders+Identification+Test%3a+guidelines+for+use+in+primary+health+care.+Generva%2c+Switzerland%3a+World+Health+Organization.+1992.+%0a%0aBeck+AT%2c+Steer+RA%2c+Carbin+MG.+Psychometric+properties+of+the+Beck+Depression+Inventory%3a+Twenty-five+years+of+evaluation.+Clinical+Psychology+Review.+1988%3b+8(1)%3a77-100.%0a%0aBelgrade+M%2c+Schamber+CD%2c+Lindgren+BR.+The+DIRE+score.+Predicting+outcomes+of+opioid+prescribing+for+chronic+pain.+The+Journal+of+Pain.+2006%3b+7(9)%3a+671-81.+Available+at%3a+http%3a%2f%2fwww.jpain.org%2farticle%2fS1526-5900(06)00626-2%2fabstract.+Accessed+on%3a+2017-04-10.+%0a%0aBriley+M%2c+Moret+C.+Treatment+of+comorbid+pain+with+serotonin+norepinephrine+reuptake+inhibitors.+CNS+Spectr.+2008%3b+13(7)%3a+22-26.%0d%0a%0aDowell+D%2c+Haegerich+TM%2c+Chou+R.+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016.+MMWR+Recomm+Rep.+2016%3b+ePub%3a+March+2016%3a+DOI%3a+http%3a%2f%2fdx.doi.org%2f10.15585%2fmmwr.rr6501e1er.+Available+at%3a+https%3a%2f%2fwww.cdc.gov%2fmmwr%2fvolumes%2f65%2frr%2frr6501e1.htm.+Accessed+on%3a+2017-04-10.%0a%0aFDA.+Drug+Safety+and+Availability+-+FDA+Drug+Safety+Communication%3a+FDA+recommends+against+the+continued+use+of+propoxyphene.+https%3a%2f%2fwww.fda.gov%2fDrugs%2fDrugSafety%2fucm234338.htm.+Accessed+April+7%2c+2017.+%0a%0aKellgren+JH%2c+Lawrence+JS.+Radiological+assessment+of+osteo-arthrosis.+Ann.+Rheum.+Dis.+2000%3b16+(4)%3a+494-502.+Available+at+https%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpubmed%2f13498604.+Accessed+5%2f29%2f2017.%0a%0aKrebs+EE%2c+Lorenz+KA%2c+Bair+MJ.+Development+and+initial+validation+of+the+PEG%2c+a+three-item+scale+assessing+pain+intensity+and+interference.+J+Gen+Intern+Med.+2009%3b+24(6)%3a+733-738.+Available+at%3a+http%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpmc%2farticles%2fPMC2686775%2f+Accessed+on%3a+2017-04-10.%0a%0aKumar+L%2c+Barker+C%2c+Emmanuel+A.+Opioid-induced+constipation%3a+Pathophysiology%2c+clinicial+consequences%2c+and+management.+Gastroenterology+Research+and+Practice.+2014%3b+2014.+Available+at%3a+https%3a%2f%2fwww.hindawi.com%2fjournals%2fgrp%2f2014%2f141737%2f.%0a%0aMarshall%2c+PS.+Physical+Functional+Ability+Questionnaire+(FAQ5).+In%3a+Assessment+and+Management+of+Chronic+Pain%2c+5th+edition.+Institute+for+Clinical+Systems+Improvement+.+2011%3b+14%3a+Appendix+C%3a99.+Available+at%3a+http%3a%2f%2fwww.generationsprimarycare.com%2fassets%2fpain-contract.pdf+Accessed+on%3a+2017-04-10.%0a%0aMerrill+JO%2c+Von+Korff+M%2c+Banta-Green+CJ%2c+et+al.+Prescribed+opioid+difficulties%2c+depression+and+opioid+dose+among+chronic+opioid+therapy+patients.+General+Hospital+Psychiatry.+2012%3b+34%3a+581-587.+Available+at%3a+http%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpubmed%2f22959422+Accessed+on%3a+2017-04-17.%0a%0aPani+PP%2c+Vacca+R%2c+Troqu+E%2c+Amato+L%2c+Davoli+M.+Pharmacological+treatment+for+depression+during+opioid+agonist+treatment+for+opioid+dependence.+Cochrane+Database+of+Systematic+Reviews.+2010%3b+8(9)%3a+CD008373.+Available+at%3a+http%3a%2f%2fonlinelibrary.wiley.com%2fdoi%2f10.1002%2f14651858.CD008373.pub2%2fabstract+Accessed+on%3a+2017-04-10.%0a%0aPassik+SD%2c+Kirsh+KL%2c+Casper+D.+Addiction-related+assessment+tools+and+pain+management%3a+instruments+for+screening%2c+treatment+planning+and+monitoring+compliance.+Pain+Med.+2008%3b+9%3a+S145-S166.%0d%0a%0d%0aWebster+LR.+Predicting+aberrant+behaviors+in+opioid-treated+patients%3a+Preliminary+validation+of+the+opioid+risk+tool.+Pain+Medicine.+2005%3b6(6)%3a432-442.+Available+at%3a+https%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpubmed%2f16336480.+Accessed+on%3a+2017-04-10.</value>
                </values>
              </panel>
            </panels>
          </tab>
          <tab id=""Post-Test"">
            <type>QuizTab</type>
            <name>Post-Test</name>
            <panels>
              <panel id=""0188cf0a31"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">%3cb%3eCase+Scenario%3c%2fb%3e%0aEllen+Small%2c+age+37%2c+has+been+taking+hydrocodone+without+a+prescription.+She+started+by+taking+opioids+she+had+left+over+from+dysmenorrhea+that+resolved+two+years+ago.+She+took+them+to+enjoy+the+euphoria+and+for+occasional+backache+or+tension+headaches.+Eventually%2c+she+found+she+had+to+take+it+all+the+time+to+avoid+feeling+%22flu-like.%22+%0a%0aAt+today%27s+office+visit%2c+Ms.+Small+contradicted+herself+during+history+taking.+To+make+sure+she+wasn%27t+trying+to+get+a+hydrocodone+prescription+for+pain+she+does+not+really+have%2c+she+was+confronted+about+the+contradiction.+She+admitted+this+was+the+case.+She+also+admitted+that+she+spends+too+much+money+and+time+obtaining+the+medication+illegally+and+that+she+wants+to+quit+but+cannot.+%0a%0aWith+just+this+much+information%2c+what+can+you+say+about+her+status+with+respect+to+developing+opioid+use+disorder%3f++(Choose+the+best+answer)</value>
                  <value id=""Image"">6fbf0853de</value>
                  <value id=""OptionTypeValue"">Multiple+Choice</value>
                </values>
                <panels>
                  <panel id=""2005e5b159"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Borderline+opioid+use+disorder</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Ms.+Small+meets+at+least+four+criteria%3a%0a1.+Taking+opioids+for+longer+than+intended%0a2.+Withdrawal+symptoms%0a3.+Spending+a+lot+of+time+obtaining+the+medication%0a4.+Wanting+to+quit+but+not+being+able+to+so.+%0aOnly+two+criteria+are+needed+for+a+diagnosis+of+opioid+use+disorder.</value>
                    </values>
                  </panel>
                  <panel id=""a04e4c2910"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Meets+enough+criteria+for+diagnosis+of+opioid+use+disorder</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Ms.+Small+meets+at+least+four+criteria%3a%0a1.+Taking+opioids+for+longer+than+intended%0a2.+Withdrawal+symptoms%0a3.+Spending+a+lot+of+time+obtaining+the+medication%0a4.+Wanting+to+quit+but+not+being+able+to+so.+%0aOnly+two+criteria+are+needed+for+a+diagnosis+of+opioid+use+disorder.</value>
                    </values>
                  </panel>
                  <panel id=""b4e8a4698a"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Does+not+meet+enough+criteria+for+diagnosis+of+opioid+use+disorder</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Ms.+Small+meets+at+least+four+criteria%3a%0a1.+Taking+opioids+for+longer+than+intended%0a2.+Withdrawal+symptoms%0a3.+Spending+a+lot+of+time+obtaining+the+medication%0a4.+Wanting+to+quit+but+not+being+able+to+so.+%0aOnly+two+criteria+are+needed+for+a+diagnosis+of+opioid+use+disorder.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
              <panel id=""a6d3067044"">
                <type>QuizTabQuestion</type>
                <values>
                  <value id=""QuestionValue"">Which+patients+being+prescribed+opioids+should+have+urine+drug+testing%3f+(Choose+the+best+answer)</value>
                  <value id=""OptionTypeValue"">Multiple+Choice</value>
                </values>
                <panels>
                  <panel id=""c207b57613"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Only+patients+being+prescribed+high+dose+opioids</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</value>
                    </values>
                  </panel>
                  <panel id=""7de455c8a7"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Only+new+patients+being+prescribed+opioids+for+chronic+pain</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</value>
                    </values>
                  </panel>
                  <panel id=""dd0e1f482d"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Only+patients+being+prescribed+any+opioids+who+have+aberrant+drug-related+behavior</value>
                      <value id=""OptionTypeValue"">Incorrect</value>
                      <value id=""FeedbackValue"">Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</value>
                    </values>
                  </panel>
                  <panel id=""5d5e29b53b"">
                    <type>QuizQuestionOption</type>
                    <values>
                      <value id=""OptionValue"">Any+patient+being+prescribed+opioids+for+chronic+pain</value>
                      <value id=""OptionTypeValue"">Correct</value>
                      <value id=""FeedbackValue"">Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</value>
                    </values>
                  </panel>
                </panels>
              </panel>
            </panels>
          </tab>
        </tabs>
      </section>
    </sections>
  </new>
</data>
";


    string legacyImgData1 = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<Body>
  <image0>
    <key>Patient_IntroSection</key>
    <imgData>
      <iconColor>0.07843138,0.6980392,0.6392157,1</iconColor>
      <reference>IconPanel1</reference>
    </imgData>
  </image0>
  <image1>
    <key>fcbdfc605c</key>
    <imgData>
      <width>32</width>
      <height>32</height>
      <data>iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAJvUlEQVRYCZ1Xa0xb1x0/914bfH1tYxsDNgZC0kENeTQ8A2FJk67bpEl7pJP2bWqnrV8mbZrUVdM2rVL3YVM6RamWpGq2al37YWmSttPStEm6rB3LslLKIxQIYB4GkwDhYePn9eveu9//gilZSdTuiKNzfO7//H+//+ucA8fW23sdHaImy0+yUOgJzmC48x9VPflMIHBJ1TQNItQ/Vxs5fJjTVHWnYWnpV3wiUR+SpFe+PjHx6p1MZhmK1LwygSbvHzgg8oLwk9KGht95Hn3U49i58wsVBsNhKR6PdEejg0BX8hs+yzhy6BCBN0lu90vOhx/+sq2jo9QqSV/Zy1jB35eWhmRVjUOPbpQwdPSoOTU5+ZTL5/u1xePhtViM8RzHbB6PtcZk2lcQjcZ7QQKUc58FfPTgQZ5pWovZ6z3tqK9vFOBALRJhRoeDeUymtl2M8VdBIq2qCSIhfHtl5fuuurqjUkmJQU0kGAdwTlX10erxSHUmU4sxHE70xeMDcMN9SYwAHJbvt7rdL9rr6/fy0IOwwuGgn0wyo83Gyo3Gth2xWOqdSGQQ+lLCEzz/ckl7u1uLxxkPtmQ9keBzuQ0Su8zmFiEUSvbH4x+DwZYkRtrbecbzX5S83lNF9fV7eDKPwBXAUIc+DSQK4Qk2OVn151CoE6vLBsQ+Sew4EtLzDT6ikYcKKFCxXuzzuX7Ecc/wnZ3CC3NzLyY1Lbk5HCP79pHbD5nLyk44fL568uCG5QDWwckg6M2trrKhXG4FCCXQIQpNXm/WzfMHRatVJJeREIcvZAF1nRhJlpWJe63WVsPqaqovFhvIMoY/xhY7OnhZUQ6bKytP2Ovq6nX5vOUEDgO0bFbXS2PnjRuzv11YuDCfTg9i+22hR9NGrImEsqOgoEkqKhL1+BMRCsU6GbKAFCFPxCaLpSUXDqcQjn6y7emqqi8JXu+JotraegobeVOPOcB0ywmcmGYy7Nrw8O2nR0fP34zFrmHlJnpYSMqy2hmN9rkUJVtjNDab4QmyQicCAjzNiQgpxFhIJKzWZnl1NX6wqMh9yOc7ZnrggTraQ26nfQyyGpGhPQglEbs2ODj389HRN24mk/8C8A30efSsfg6Aca4rHr9hk2WlXhQbRUkSyQqdBKTyZEghARXYbGKrJO1vLS39RmFVVSUBaFRBRAI9n3TYygR8+/fQ0Pwvx8b+OpBI/BNLBH4bXQ/hGgH8UuCB68lkXzHI1BkMjSYisa5MJ4C5bh1cSUAmh6NQdDpFcq3eETI9ieGlPHkBhD+cmFj4xdjYhb5E4j3A9G0Gx5xtEKAfqOFsVyLRL8pybndhYaNJFEUOFlDfUKoLAoSAyMXpNKo5xaiMWTTKNGS5FgoxHuvdc3OLz05NvdUViVyFfD+23kIH40+a4ZPp2iyVTieem58/KRoM/HczmacsLpcTxPRYckbjmnthtULJRoA4OVk8hhqXmbbuDQ3euOnxLD6XSl26Hon8A/vJ8k+BE+JdHlijQMaqmf50esCJ6XZNe6jQaDRTMqorK0ydmWHK+DhTJyaYGggw9c4d3Wo6RTV4IpnNqjeLi5dOS9LVq4uLlzOy3A29s+h3WZ7H2pIAfUxls+kPM5mB7U5ndruiNBv9fpMyNcXUuTnGyGrKBchRuVKmIyBMKChgyzU1sZNFRZcv3rr1TjIavS844dBZc88WjcejpbW1t4R4XFFmYQRlOqQ5AFIHOiN3I0AM1zbLIUSWbdsM210uDeBDWJ5G39JyrOvtnh6gr9qTT36remXlWW1mpkpFkm1YvLZXt5rAUXj6BZEGCZOmGRvd7nKDw7Hcv7zcl8vl/j8CsdOnj8S6uo5l/f4a5TbKlsqQ3L3eyOW65RhRI0zBtxxkkiBawHHm9urqPcaSkuT12dkBvGlIZMu2pQdiJ04cSZ07d4xbWtqhTk8zDiWlu3xdhQ4Oa8lyAlbI/ZhnMc+skzCYTFKD19sg2O2JrtnZj/F5SxKfIhA9depI+s03j3PRaLXi968drxTv9UZW0yuNCOUAmEWJ6iPW6WijOSqBIQeYRRSl5vLyRsHpXAUJunyI813tLgLR559/LHPx4nEuHt+mDA/r9/dmy3VwAKYtFiXc2hpX7HaOW1wUKPYETMGmrmKeRpUkw2Fms9stDR5PE+9whLqDwWF8vovEBoHYqVOPZd5663mAVymDg/cEV8xmFmltjUV8vnm1rCyEi8qSW1oy4omlk9DDAhSqDhkkEjg77EVF1l2lpS1CcfHCR8HgyGYSOoHlxx8/ovb3/x7glcrAwFq5/a/bAQBwTe7okKc8nqnfdHa+Es7lPvjagQOusCyXpBYXDQpkyPp8ZegkkD9xeKLE6bT6nM5mJOZcz8yMP09CiB09+lU8kV7AKVepApxuNf01BAlqeqaTdQS+f78c8HonX/zoo7NXxsfPD87PXy+x2VYeaWurXJbl0sw6CUpMfR/2E4kUTsgoPFFaXFxUAxJZSQoMLywEkUtZ4acOx8va0tJuta9Pd3sePF9mdA/o4O3tyany8qkXurvPXPH730DmB5Hx8gfB4KTbbg8dbmmpXAGJ9OKicbMndG9QOECCwuF2uewVLtf2835/J3Ss8szhqGY43+nBuBlcJ7AGzpIAD3q9gT/29Jy9DPCMqk7DOP2AWU2n0z+7cuXi2dHR43WPPNItNjamVEFgOSRm3gs0kifCqIwF3CHVNttugeN2YNliUK3WG7naWpsQDBYRaL7T0aqIopZoa0vOlJcH/tDTc+Ztv/91lN1MHhyj3iIg8fSVK29TsX7z0KEfD/F8a7i316zgGqec0BMT+qhlvd50sqAghF+4Wpkg/LCpKcBcrla8ZOzc/HxBnnXWZNLktrbETGXlNFl+cWzs/Do4HgCfbgiH8v709NQOu32lo6mpPKaqpQnoy1FiQpzgLQ0NKavPt3RmaOgNvBU6kQMLAn4sfO+hh1a08vI6CFn55eUCJAlLtLYmghUVgZd6e89dGBs7B0VB6NgSPE+HSFwNBKZqUPPte/d6IyCRBAnKCeuePbJj9+7QhfHxC3/q738NsmPYlxASuL9fGx6e+kFzczLrdm9bLS7Oxaur5YWSktuv9Pe//iZesVBA4Kk80P1GeEl9d3Jyss7lCrft2uWNSZLEV1RkHA8+uPpuIHD5dG/vGWDSTYk7nan6OYCF3F+Ghsa+s3Nn0OR0lq8Iwq1XBwZeOzs8/DdkKr1kPhN4nhi8pV4aH5/wuVyL+2prSySHI/M2LD/Z3X02nsnQczxK4CT/yfWGH0aeN+DKMyM2JiRhDp3+i9WznYQ/b0OmCzzPS9hnASe6tAgY5bYGTvr+CyX0v5pf4EztAAAAAElFTkSuQmCC</data>
    </imgData>
  </image1>
</Body>";

    string legacyData2 = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<body>
  <SaveCaseBG>
    <PatientNameValue>Chad Wright</PatientNameValue>
    <PrivateToggle>True</PrivateToggle>
    <OverwriteTemplateToggle>True</OverwriteTemplateToggle>
    <DescriptionValue>Chronic Knee Pain</DescriptionValue>
    <SummaryValue>Chad Wright is a 35 yo male presenting with chronic knee pain that started with a high school football injury. His pain management has included chronic opioid therapy in recent years. He now presents as a new patient requesting a prescription for opioids. </SummaryValue>
    <TagsValue>Pain Management; </TagsValue>
    <TargetAudienceValue>MD/DO/PA/NP</TargetAudienceValue>
    <DifficultyValue>Intermediate</DifficultyValue>
  </SaveCaseBG>
  <CharacterEditorPanel>
    <CharacterNameValue>
    </CharacterNameValue>
    <BMIValue>34.6</BMIValue>
    <HeightValue>70</HeightValue>
    <HeightSlider>70</HeightSlider>
    <WeightValue>241</WeightValue>
    <WeightSlider>241</WeightSlider>
    <AgeValue>93</AgeValue>
    <AgeSlider>93</AgeSlider>
    <Face1Toggle>True</Face1Toggle>
    <FaceSlider>1</FaceSlider>
    <HairSlider>1</HairSlider>
    <Hair1Toggle>True</Hair1Toggle>
  </CharacterEditorPanel>
  <Sections>
    <_PATIENT_INTROSection>
      <sectionName>
      </sectionName>
      <TextboxTab>
        <customTabName>Case Overview</customTabName>
        <data>
          <EntryData>
            <Parent>PATIENT_INTROSection/Case OverviewTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <DialoguePin>
                  <dialogue>
                    <uid>PATIENT_INTROSection/Case OverviewTab/LabEntry: 0</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>PATIENT_INTROSection/Case OverviewTab/LabEntry: 0</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Instructor</characterName>
                            <charColor>RGBA(0.569, 0.569, 0.569, 1.000)</charColor>
                            <dialogueText>%5bAnswers+phone%5d+Hello.+Smitherton+Clinic.+How+may+I+help+you%3f</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+need+an+appointment+to+get+some+more+pain+medication+for+my+knee.</dialogueText>
                          </PanelData>
                        </Entry1>
                        <Entry2>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Instructor</characterName>
                            <charColor>RGBA(0.569, 0.569, 0.569, 1.000)</charColor>
                            <dialogueText>Okay.+And+have+you+been+here+before%3f</dialogueText>
                          </PanelData>
                        </Entry2>
                        <Entry3>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>No%2c+I%27m+a+new+patient.+</dialogueText>
                          </PanelData>
                        </Entry3>
                        <Entry4>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Instructor</characterName>
                            <charColor>RGBA(0.569, 0.569, 0.569, 1.000)</charColor>
                            <dialogueText>Okay.+I+have+an+appointment+available+next+Tuesday+at+10+am.</dialogueText>
                          </PanelData>
                        </Entry4>
                        <Entry5>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+can%27t+wait+that+long.+I%27ve+run+out+of+my+prescription.+Can+you+fit+me+in+sooner%3f</dialogueText>
                          </PanelData>
                        </Entry5>
                        <Entry6>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Instructor</characterName>
                            <charColor>RGBA(0.569, 0.569, 0.569, 1.000)</charColor>
                            <dialogueText>Well%2c+we+could+fit+you+in+tomorrow+at+8+am.+</dialogueText>
                          </PanelData>
                        </Entry6>
                        <Entry7>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Okay.+I%27ll+take+it.+Thanks%21</dialogueText>
                          </PanelData>
                        </Entry7>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <CustomContentValue>%3cb%3eNew+Patient%3a+Chad+Wright%2c+age+34%3c%2fb%3e%0a%0a%3cb%3ePresenting+Problem%3a%3c%2fb%3e+Mr.+Wright+is+looking+for+a+new+provider+to+prescribe+pain+medication+for+an+old+knee+injury.%0a%0a%3cb%3eScenario%3a%3c%2fb%3e+Chad+Wright+has+chronic+knee+pain+for+which+he+has+been+taking+an+opioid+analgesic.+He+is+currently+out+of+medication+and+looking+for+a+new+provider.+Finding+providers+who+are+willing+to+continue+prescribing+opioids+for+him+has+been+a+struggle+because+of+their+concerns+he+might+become+addicted.+%0a%0aMr.+Wright+traces+his+pain+back+to+a+football+injury+in+college.+Now+he+is+entering+middle+age+and+is+the+father+of+two+young+boys.+He+has+been+through+many+different+forms+of+treatment%2c+including+surgery+in+his+twenties+and+opioids%2c+which+he+has+taken+for+many+years.+Despite+these+treatments%2c+his+pain+continues+to+be+a+problem+if+he+doesn%27t+take+opioids+every+day.+He+would+like+to+be+more+active+with+his+children+and+wants+to+appear+stronger+in+front+of+them.+The+pain+and+limitations+contribute+to+being+depressed%2c+which+is+being+treated+with+antidepressants.+%0a%0aMr.+Wright+presents+to+this+medical+practice+as+a+new+patient%2c+requesting+a+prescription+for+the+long-acting+opioids+that+he+has+been+taking.</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <Personal_InfoTab>
        <customTabName>Personal Info</customTabName>
        <data>
          <EntryData>
            <Parent>PATIENT_INTROSection/Personal InfoTab/</Parent>
            <Entry0>
              <PanelType>BasicDetailsPanel</PanelType>
              <PanelData>
                <RecordValue>094118</RecordValue>
                <FirstNameValue>Chad</FirstNameValue>
                <LastNameValue>Wright</LastNameValue>
                <Gender>Male</Gender>
                <MonthValue>09</MonthValue>
                <DayValue>03</DayValue>
                <YearValue>1983</YearValue>
                <AgeValue>34</AgeValue>
                <AllergiesValue>Penicillin</AllergiesValue>
                <Education>Bachelor%27s+Degree</Education>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>AdditionalPanel</PanelType>
              <PanelData>
                <AdditionalInformationValue>Limited+mobility+due+to+chronic+knee+pain.+Uses+a+cane+periodically.+</AdditionalInformationValue>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </Personal_InfoTab>
      <Office_VisitTab>
        <customTabName>Office Visit</customTabName>
        <data>
          <EntryData>
            <Parent>PATIENT_INTROSection/Office VisitTab/</Parent>
            <Entry0>
              <PanelType>OfficeVisitPanel</PanelType>
              <PanelData>
                <BPValue1>130</BPValue1>
                <BPValue2>80</BPValue2>
                <PulseValue>85</PulseValue>
                <TempValue>98.4</TempValue>
                <RespValue>16</RespValue>
                <HeightValue>70</HeightValue>
                <WeightValue>192</WeightValue>
                <BMIValue>27.5+kg%2fm%c2%b2</BMIValue>
                <ChiefComplaintValue>I+need+a+prescription+for+oxycodone+for+my+knee+pain+since+my+last+doctor+stopped+prescribing+it.</ChiefComplaintValue>
                <PainValue>6</PainValue>
                <HoPIValue>Chronic+knee+pain+for+over+15+years+for+which+he+has+been+taking+an+extended-release+opioid.+History+of+depression%2c+which+has+been+fairly+well-managed+with+antidepressant+medication.+%0a%0aHis+pain+is+fairly+well-controlled+by+taking+opioids%2c+but+sometimes+increases+after+periods+of+activity.+He+was+taking+his+next+dose+a+little+early+whenever+his+pain+worsened+and%2c+as+a+result%2c+he+ran+out+of+his+medications+early.+After+requesting+the+medication+early+several+times%2c+his+previous+provider+refused+to+write+another+prescription.+</HoPIValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </Office_VisitTab>
    </_PATIENT_INTROSection>
    <_HistorySection>
      <sectionName>
      </sectionName>
      <TextboxTab>
        <customTabName>Hx Info</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/Hx InfoTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3cb%3e%3ccolor%3d%23FFBE45%3eBACKGROUND+INFORMATION+FOR+HISTORY%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cb%3ePain+History%3c%2fb%3e%0a%0aBecause+pain+is+a+subjective+experience%2c+tools+such+as+pain+rating+scales+can+be+helpful+in+turning+the+patient%27s+experience+into+something+measurable%2c+such+as+a+rating+on+a+10-point+scale.+Be+sure+to+also+explore+the+patient%27s+emotional+and+cognitive+components+of+pain%2c+for+example%2c+by+asking+how+the+pain+is+affecting+their+life.+%0a%0aWhen+interviewing+a+patient+about+acute+or+chronic+pain+history%2c+an+acronym+such+as+the+PQRSTU+acronym+can+help%3a%0a%0a%3cb%3e%3ccolor%3d%23008080FF%3eP%3a%3c%2fcolor%3e%3c%2fb%3e+What+provokes+the+pain%3f+What+palliation+have+they+tried%3f+What+is+the+past+history+of+similar+pain%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eQ%3a%3c%2fcolor%3e%3c%2fb%3e+What+is+the+quality+of+the+pain%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eR%3a%3c%2fcolor%3e%3c%2fb%3e+Does+the+pain+radiate%3f++What+region+of+the+body+is+involved%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eS%3a%3c%2fcolor%3e%3c%2fb%3e+How+severe+is+the+pain%3f+Using+a+scale+of+1+to+10+can+help.%0a%3cb%3e%3ccolor%3d%23008080FF%3eT%3a%3c%2fcolor%3e%3c%2fb%3e+Timing+Factors%3a+Onset+-+When+did+it+start%3f+When+does+it+occur+now%3f+Any+patterns%3f+Duration%3f%0a%3cb%3e%3ccolor%3d%23008080FF%3eU%3a%3c%2fcolor%3e%3c%2fb%3e+How+does+the+pain+affect+you%3f+Include+impact+on+psychosocial+and+mechanical+functioning.+%0a%0a%3cb%3eOpioid+Prescribing+Guidelines%3c%2fb%3e%0aCurrent+opioid+prescribing+guidelines+published+by+the+CDC+for+chronic+pain+treatment+include+the+following+recommendations%3a%0a1.+Use+other+non-opioid+and+non-pharmacological+treatments+first+if+possible%0a2.+Prescribe+opioids+only+for+moderate+to+severe+pain+that+cannot+be+managed+by+other+treatments.%0a3.+If+opioids+are+prescribed%2c+only+a+3-day+supply+is+sufficient+for+most+acute+pain.+Prescriptions+should+last+for+the+duration+of+the+most+severe+pain%2c+not+for+the+duration+of+the+pain.%0a(Dowell+et+al.%2c+2016)%0a%0a%3cb%3eRegarding+past+pain+medications+described+in+this+case%3a%3c%2fb%3e%0aThe+FDA+recommended+against+the+continued+use+of+propoxyphene+(Darvon)+in+2010+because+of+heart+toxicity+(FDA%2c+2010).</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <Review_Of_SystemsTab>
        <customTabName>ROS</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/ROSTab/</Parent>
            <Entry0>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>General%2c+constitutional</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Eyes%2c+vision</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Ears%2c+nose%2c+throat</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Cardiovascular</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry3>
            <Entry4>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Respiratory</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry4>
            <Entry5>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <CollapseCheckBoxToggle>True</CollapseCheckBoxToggle>
                <PanelNameValue>Gastrointestinal</PanelNameValue>
                <DialoguePin>
                  <dialogue>
                    <uid>HistorySection/ROSTab/LabEntry: 5</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>HistorySection/ROSTab/LabEntry: 5</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I+see+you+have+some+constipation.+Is+that+still+a+problem%3f</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>It%27s+not+much+of+a+problem+anymore.+It%27s+only+if+I+don%27t+take+a+stool+softener+and+eat+enough+fiber.</dialogueText>
                          </PanelData>
                        </Entry1>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <DetailsValue>Opioid-induced+constipation.+</DetailsValue>
              </PanelData>
            </Entry5>
            <Entry6>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Genitourinary</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry6>
            <Entry7>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <CollapseCheckBoxToggle>True</CollapseCheckBoxToggle>
                <PanelNameValue>Musculoskeletal</PanelNameValue>
                <DialoguePin>
                  <dialogue>
                    <uid>HistorySection/ROSTab/LabEntry: 7</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>HistorySection/ROSTab/LabEntry: 7</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I+see+you+rated+your+pain+as+a+6+out+of+10+in+your+intake+form.+How+badly+does+it+hurt+at+different+times+of+the+day%3f</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>It+depends+more+on+how+active+I+am%2c+which+makes+it+worse%2c+and+whether+or+not+I%27m+taking+an+opioid%2c+which+gets+the+pain+down+to+around+a+2+out+of+10.+</dialogueText>
                          </PanelData>
                        </Entry1>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <DetailsValue>Right+knee+pain</DetailsValue>
              </PanelData>
            </Entry7>
            <Entry8>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Integumentary+%26+breast</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry8>
            <Entry9>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <CollapseCheckBoxToggle>True</CollapseCheckBoxToggle>
                <PanelNameValue>Neurological</PanelNameValue>
                <DialoguePin>
                  <dialogue>
                    <uid>HistorySection/ROSTab/LabEntry: 9</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>HistorySection/ROSTab/LabEntry: 9</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>And+you%e2%80%99re+taking+duloxetine+for+depression%2c+I+see.+How+well+is+that+working%3f</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Pretty+well.</dialogueText>
                          </PanelData>
                        </Entry1>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <DetailsValue>Mild+depression%2c+takes+Cymbalta%c2%ae+(duloxetine)</DetailsValue>
              </PanelData>
            </Entry9>
            <Entry10>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Psychiatric</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry10>
            <Entry11>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Endocrine</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry11>
            <Entry12>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Hematologic+%2f+lymphatic</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry12>
            <Entry13>
              <PanelType>ReviewOfSystemsPanel</PanelType>
              <PanelData>
                <PanelNameValue>Allergic+%2f+immunologic</PanelNameValue>
                <DetailsValue>
                </DetailsValue>
              </PanelData>
            </Entry13>
          </EntryData>
        </data>
      </Review_Of_SystemsTab>
      <Medical_EventsTab>
        <customTabName>Med Events</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/Med EventsTab/</Parent>
            <Entry0>
              <PanelType>MedicalEventEntryPanel</PanelType>
              <PanelData>
                <DialoguePin>
                  <dialogue>
                    <uid>HistorySection/Med EventsTab/LabEntry: 0</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>HistorySection/Med EventsTab/LabEntry: 0</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>It+sounds+like+you%27ve+been+dealing+with+your+knee+pain+for+a+while.+When+did+it+start%3f</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I%27ve+had+pain+since+a+football+injury%2c+around+15+years+ago.</dialogueText>
                          </PanelData>
                        </Entry1>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <ProblemEventValue>Anterior+cruciate+ligament+(ACL)+surgically+repaired+right+knee</ProblemEventValue>
                <OneTimeToggle>True</OneTimeToggle>
                <StartMonthValue>09</StartMonthValue>
                <StartDayValue>03</StartDayValue>
                <StartYearValue>2002</StartYearValue>
                <DetailsValue>Sports-related+injury+surgically+repaired.+Physician+Alfred+Chen%2c+MD.+%0a+%e2%80%a2+Post-surgical+function+good.+Post-surgical+pain+treated+with+oxycodone+plus+acetaminophen.%0a+%e2%80%a2+Physical+therapy+for+4+weeks%2c+once+per+week.+%0a+%e2%80%a2+Post-surgical+neuropathic+pain+continued%2c+treated+with+chronic+opioid+therapy.+%0a+%e2%80%a2+Constipation%2c+opioid-induced%2c+managed+with+stool+softener+and+laxative.+</DetailsValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>MedicalEventEntryPanel</PanelType>
              <PanelData>
                <ProblemEventValue>Endoscopic+surgical+removal+of+scar+tissue%2c+right+knee</ProblemEventValue>
                <OneTimeToggle>True</OneTimeToggle>
                <StartMonthValue>03</StartMonthValue>
                <StartDayValue>07</StartDayValue>
                <StartYearValue>2009</StartYearValue>
                <DetailsValue>Mild+improvement+in+post-surgical+neuropathic+pain+(Dr.+Jared+Smith).+Physical+therapy+was+recommended+for+two+months%2c+but+the+patient+self-discontinued+PT+after+one+month.+</DetailsValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>MedicalEventEntryPanel</PanelType>
              <PanelData>
                <DialoguePin>
                  <dialogue>
                    <uid>HistorySection/Med EventsTab/LabEntry: 2</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>HistorySection/Med EventsTab/LabEntry: 2</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Did+the+counseling+help+you+cope+with+the+pain+and+stress%3f</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+went+a+few+times+and+learned+about+managing+stress+and+my+feelings+about+having+pain.+And+it+did+help.+Taking+an+antidepressant+helps%2c+too.++</dialogueText>
                          </PanelData>
                        </Entry1>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <ProblemEventValue>Cognitive+Behavioral+Therapy+(CBT)</ProblemEventValue>
                <PeriodOfTimeToggle>True</PeriodOfTimeToggle>
                <StartMonthValue>04</StartMonthValue>
                <StartDayValue>03</StartDayValue>
                <StartYearValue>2009</StartYearValue>
                <EndMonthValue>04</EndMonthValue>
                <EndDayValue>24</EndDayValue>
                <EndYearValue>2012</EndYearValue>
                <DetailsValue>For+help+in+coping+with+pain+(3+sessions).</DetailsValue>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>MedicalEventEntryPanel</PanelType>
              <PanelData>
                <ProblemEventValue>Hyaluronic+acid+injection%2c+right+knee</ProblemEventValue>
                <OneTimeToggle>True</OneTimeToggle>
                <StartMonthValue>07</StartMonthValue>
                <StartDayValue>03</StartDayValue>
                <StartYearValue>2012</StartYearValue>
                <DetailsValue>Provided+mild+pain+relief+for+several+weeks.</DetailsValue>
              </PanelData>
            </Entry3>
            <Entry4>
              <PanelType>MedicalEventEntryPanel</PanelType>
              <PanelData>
                <ProblemEventValue>Mild+depression</ProblemEventValue>
                <OngoingToggle>True</OngoingToggle>
                <StartMonthValue>06</StartMonthValue>
                <StartDayValue>17</StartDayValue>
                <StartYearValue>2003</StartYearValue>
                <DetailsValue>Multiple+episodes+of+mild+depression+due+to+pain+and+loss+of+ability+to+be+active+and+play+sports.+Treated+with+antidepressant%2c+duloxetine.</DetailsValue>
              </PanelData>
            </Entry4>
            <Entry5>
              <PanelType>MedicalEventEntryPanel</PanelType>
              <PanelData>
                <ProblemEventValue>Cortisone+injections%2c+right+knee</ProblemEventValue>
                <PeriodOfTimeToggle>True</PeriodOfTimeToggle>
                <StartMonthValue>11</StartMonthValue>
                <StartDayValue>11</StartDayValue>
                <StartYearValue>2003</StartYearValue>
                <EndMonthValue>08</EndMonthValue>
                <EndDayValue>24</EndDayValue>
                <EndYearValue>2011</EndYearValue>
                <DetailsValue>Multiple+cortisone+injections+in+the+first+8+years+after+the+initial+knee+injury.</DetailsValue>
              </PanelData>
            </Entry5>
          </EntryData>
        </data>
      </Medical_EventsTab>
      <Family_Social_HistoryTab>
        <customTabName>Fam Soc Hx</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/Fam Soc HxTab/</Parent>
            <Entry0>
              <PanelType>FamilyHistoryPanel</PanelType>
              <PanelData>
                <FamilyHistoryValue>Family+Health+History</FamilyHistoryValue>
                <ChronicDiseaseValue>Relatives%3a+Mother+(age+65)+%e2%80%93+Hypertension%3b+Father+(age+65)+%e2%80%93+Hyperlipidemia%3b+Grandfather+%e2%80%93+Coronary+artery+disease%2c+deceased+from+MI+at+age+67.</ChronicDiseaseValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>SocialHistoryPanel</PanelType>
              <PanelData>
                <MaritalStatusValue>Married</MaritalStatusValue>
                <ChildrenValue>2</ChildrenValue>
                <SpousePartnerValue>Ann</SpousePartnerValue>
                <SocialSupportsValue>Wife%2c+Ann%2c+and+a+few+%22drinking+buddies%22</SocialSupportsValue>
                <OccupationValue>Insurance+adjuster</OccupationValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>SubstanceUsePanel</PanelType>
              <PanelData>
                <TobaccoNotAvaliable>
                </TobaccoNotAvaliable>
                <TobaccoCurrentUseToggle>True</TobaccoCurrentUseToggle>
                <TobaccoDetailsValue>Smokes+cigarettes.+History+of+smoking+for+28+pack%2fyears+(avg.+of+2+packs+per+day+for+14+years)%0d.+</TobaccoDetailsValue>
                <AlcoholNotAvaliable>
                </AlcoholNotAvaliable>
                <AlcoholCurrentUseToggle>True</AlcoholCurrentUseToggle>
                <AlcoholDetailsValue>1%e2%80%932+drinks%2fday%2c+several+days+per+week</AlcoholDetailsValue>
                <DrugNotAvaliable>
                </DrugNotAvaliable>
                <DrugNoneToggle>True</DrugNoneToggle>
                <DrugDetailsValue>Takes+opioids+not+prescribed+for+him+that+he+obtains+from+friends+when+he+cannot+get+them+from+a+doctor.+</DrugDetailsValue>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>DietExercisePanel</PanelType>
              <PanelData>
                <DietValue>Diet+Survey+past+due</DietValue>
                <ExerciseValue>Limited+due+to+knee+pain+</ExerciseValue>
              </PanelData>
            </Entry3>
            <Entry4>
              <PanelType>OtherPanel</PanelType>
              <PanelData>
                <CollapseButtonCheckBox>True</CollapseButtonCheckBox>
                <OtherValue>He+feels+that+the+limitations+from+his+knee+pain+and+poor+functioning+interfere+with+having+an+active+social+life+and+playing+with+his+children%2c+which+contributes+to+his+depression.</OtherValue>
              </PanelData>
            </Entry4>
          </EntryData>
        </data>
      </Family_Social_HistoryTab>
      <Medication_HistoryTab>
        <customTabName>Meds</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/MedsTab/</Parent>
            <Entry0>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>oxycodone%2c+long-acting</MedicationValue>
                <QuizPin>
                  <data>
                    <EntryData>
                      <Parent>HistorySection/MedsTab/LabEntry: 0</Parent>
                      <Entry0>
                        <PanelType>QuizQuestion</PanelType>
                        <PanelData>
                          <QuestionValue>Select+the+best+way+to+learn+about+all+the+prescriptions+Mr.+Wright+has+had+in+the+past+year%2c+assuming+you+have+appropriate+permissions+in+place.</QuestionValue>
                          <Image>
                          </Image>
                          <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                          <data>
                            <EntryData>
                              <Parent>HistorySection/MedsTab/LabEntry: 0::LabEntry: 0</Parent>
                              <Entry0>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>Call+the+Pharmacy+number+he+gives+to+you</OptionValue>
                                  <OptionTypeValue>Partially+Correct</OptionTypeValue>
                                  <FeedbackValue>Asking+the+pharmacy+(with+appropriate+permissions)+would+yield+information+about+just+that+pharmacy.+Mr.+Wright+may+omit+other+pharmacies%2c+however.+The+Prescription+Drug+Monitoring+Program+has+the+most+reliably+complete+information+on+prescriptions+in+his+name+within+the+state.+</FeedbackValue>
                                </PanelData>
                              </Entry0>
                              <Entry1>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>Call+his+last+prescribing+provider</OptionValue>
                                  <OptionTypeValue>Partially+Correct</OptionTypeValue>
                                  <FeedbackValue>Asking+Mr.+Wright%27s+last+prescribing+provider+(with+appropriate+permissions)+will+only+give+you+information+about+what+that+provider+prescribed.+Contacting+the+past+provider+about+this+complex+case+is+important.+However%2c+Chad+may+have+withheld+information+about+other+doctors+he+has+seen.+%0a%0aThe+Prescription+Drug+Monitoring+Program+has+the+most+reliably+complete+information+on+prescriptions+in+his+name+within+the+state.+</FeedbackValue>
                                </PanelData>
                              </Entry1>
                              <Entry2>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>Consult+the+Prescription+Drug+Monitoring+Program</OptionValue>
                                  <OptionTypeValue>Correct</OptionTypeValue>
                                  <FeedbackValue>Prescription+Drug+Monitoring+Plan+reports+are+recommended+by+CDC%27s+opioid+prescribing+guidelines+for+all+patients+on+chronic+opioid+therapy%2c+before+prescribing+and+then+monthly+thereafter+or+at+each+prescription+(Dowell+et+al.%2c+2016).+Check+for%3a%0a+%e2%80%a2+Evidence+of+the+patient+seeing+more+than+one+doctor+for+the+same+condition%2c+which+may+signal+misuse+by+the+patient+or+diversion.+%0a+%e2%80%a2+The+patient+not+filling+a+prescription%2c+which+may+signal+diversion.+%0a%0aPrescription+drug+monitoring+programs+will+provide+prescribing+information+on+oxycodone+and+other+controlled+substances+Mr.+Wright+has+received+from+all+doctors+and+pharmacies+within+the+state.+The+other+answers+also+might+yield+some+valuable+information+but+are+less+likely+to+be+as+reliably+complete.%0a%0a%3cb%3eYour+PDMP+report+will+be+available+for+you+to+review+later+in+the+appointment.+%3c%2fb%3e</FeedbackValue>
                                </PanelData>
                              </Entry2>
                              <Entry3>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>Ask+Mr.+Wright</OptionValue>
                                  <OptionTypeValue>Partially+Correct</OptionTypeValue>
                                  <FeedbackValue>Asking+Mr.+Wright+might+gather+all+information+needed%2c+especially+if+you+have+built+an+open+and+honest+relationship+with+him.+However%2c+if+he+withholds+anything%2c+you+could+miss+important+information.+%0a%0aThe+Prescription+Drug+Monitoring+Program+has+the+most+reliably+complete+information+on+prescriptions+in+his+name+within+the+state.+</FeedbackValue>
                                </PanelData>
                              </Entry3>
                            </EntryData>
                          </data>
                        </PanelData>
                      </Entry0>
                    </EntryData>
                  </data>
                </QuizPin>
                <StartMonthValue>06</StartMonthValue>
                <StartDayValue>28</StartDayValue>
                <StartYearValue>2016</StartYearValue>
                <PresentToggle>True</PresentToggle>
                <DoseValue>30+mg</DoseValue>
                <HowTakenValue>Take+one+tablet+every+12+hours</HowTakenValue>
                <ConditionValue>Chronic+knee+pain</ConditionValue>
                <ResponseValue>Reduces+pain+in+half</ResponseValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>duloxetine</MedicationValue>
                <StartMonthValue>09</StartMonthValue>
                <StartDayValue>02</StartDayValue>
                <StartYearValue>2016</StartYearValue>
                <PresentToggle>True</PresentToggle>
                <DoseValue>20+mg</DoseValue>
                <HowTakenValue>Take+one+tablet+once+per+day</HowTakenValue>
                <ConditionValue>Depression+and+neuropathic+pain</ConditionValue>
                <ResponseValue>Depression+improved+from+moderate+to+mild%2c+a+little+pain+relief</ResponseValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>docusate+sodium+stool+softener</MedicationValue>
                <StartMonthValue>09</StartMonthValue>
                <StartDayValue>02</StartDayValue>
                <StartYearValue>2016</StartYearValue>
                <PresentToggle>True</PresentToggle>
                <DoseValue>One+to+three+capsules</DoseValue>
                <HowTakenValue>Take+with+water+once+per+day</HowTakenValue>
                <ConditionValue>Opioid-induced+constipation</ConditionValue>
                <ResponseValue>Controls+constipation+fairly+well</ResponseValue>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>oxycodone%2facetaminophen%2c+immediate-release</MedicationValue>
                <StartMonthValue>04</StartMonthValue>
                <StartDayValue>19</StartDayValue>
                <StartYearValue>2016</StartYearValue>
                <EndMonthValue>06</EndMonthValue>
                <EndDayValue>27</EndDayValue>
                <EndYearValue>2016</EndYearValue>
                <DoseValue>10+mg%2f325+mg</DoseValue>
                <HowTakenValue>Take+one+tablet+every+4+to+6+hours</HowTakenValue>
                <ConditionValue>Chronic+knee+pain</ConditionValue>
                <ResponseValue>Pain+reduced+to+a+tolerable+level+most+of+the+time</ResponseValue>
              </PanelData>
            </Entry3>
            <Entry4>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>hydrocodone%2facetaminophen</MedicationValue>
                <StartMonthValue>11</StartMonthValue>
                <StartDayValue>09</StartDayValue>
                <StartYearValue>2015</StartYearValue>
                <EndMonthValue>04</EndMonthValue>
                <EndDayValue>18</EndDayValue>
                <EndYearValue>2016</EndYearValue>
                <DoseValue>10+mg%2f325+mg</DoseValue>
                <HowTakenValue>Take+one+tablet+every+4+to+6+hours</HowTakenValue>
                <ConditionValue>Chronic+knee+pain</ConditionValue>
                <ResponseValue>Only+modest+pain+relief</ResponseValue>
              </PanelData>
            </Entry4>
            <Entry5>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>ibuprofen</MedicationValue>
                <StartMonthValue>06</StartMonthValue>
                <StartDayValue>21</StartDayValue>
                <StartYearValue>2011</StartYearValue>
                <PresentToggle>True</PresentToggle>
                <DoseValue>200+mg</DoseValue>
                <HowTakenValue>1-2+tabs+prn+extra+pain</HowTakenValue>
                <ConditionValue>Right+knee+chronic+pain</ConditionValue>
                <ResponseValue>Modest+pain+relief</ResponseValue>
              </PanelData>
            </Entry5>
          </EntryData>
        </data>
      </Medication_HistoryTab>
      <Test_HistoryTab>
        <customTabName>Past Med Tests</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/Past Med TestsTab/</Parent>
            <Entry0>
              <PanelType>OtherPastTestEntryPanel</PanelType>
              <PanelData>
                <TestValue>MRI%2c+right+knee</TestValue>
                <MonthValue>03</MonthValue>
                <DayValue>07</DayValue>
                <YearValue>2002</YearValue>
                <DetailsValue>Tear+of+ACL%2c+right+knee</DetailsValue>
                <InterpretationValue>ACL+rupture%2c+right+knee%2c+confirmed</InterpretationValue>
                <Image>642fb3a863</Image>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>OtherPastTestEntryPanel</PanelType>
              <PanelData>
                <TestValue>Metabolic+Panel</TestValue>
                <MonthValue>06</MonthValue>
                <DayValue>24</DayValue>
                <YearValue>2017</YearValue>
                <DetailsValue>
                </DetailsValue>
                <InterpretationValue>No+significant+positive+findings.+</InterpretationValue>
                <Image>
                </Image>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </Test_HistoryTab>
      <TextboxTab>
        <customTabName>Pain Hx</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/Pain HxTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <DialoguePin>
                  <dialogue>
                    <uid>HistorySection/Pain HxTab/LabEntry: 0</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>HistorySection/Pain HxTab/LabEntry: 0</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>What+helps+the+pain%3f</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+mostly+just+take+oxycodone+for+it.+That%27s+usually+all+I+need.+On+bad+nights%2c+I+sometimes+take+the+next+dose+of+oxycodone+a+few+hours+early%2c+but+that+got+me+in+trouble.+That%27s+why+the+last+provider+stopped+prescribing+it.+So+I+was+wondering+if+you+could+prescribe+me+some+extra+pills+for+when+my+pain+gets+worse.+++</dialogueText>
                          </PanelData>
                        </Entry1>
                        <Entry2>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <data>
                              <EntryData>
                                <Parent>HistorySection/Pain HxTab/LabEntry: 0::LabEntry: 2</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>%e2%80%9cI%27m+sorry%2c+with+the+dangerous+way+you%27ve+been+taking+opioids%2c+I+cannot+treat+you.%e2%80%9d</OptionValue>
                                    <OptionTypeValue>Incorrect</OptionTypeValue>
                                    <FeedbackValue>Patients+who+struggle+with+chronic+pain+and+risky+medication+use+still+deserve+pain+treatment.+It+may+be+safe+to+treat+them+in+your+clinic+with+additional+treatment+structure.+If+his+behavior+falls+outside+the+scope+of+your+practice%2c+however%2c+he+should+be+referred+to+an+addiction+or+pain+specialist.%0d</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>%22You+have+to+stop+managing+your+own+medications+if+you+want+to+be+treated+in+this+clinic.%22</OptionValue>
                                    <OptionTypeValue>Partially+Correct</OptionTypeValue>
                                    <FeedbackValue>It+is+critical+that+Mr.+Wright+stop+several+dangerous+behaviors+(taking+medications+early%2c+taking+opioids+not+prescribed+for+him%2c+and+taking+an+opioid+together+with+alcohol).+These+behaviors+increase+the+risk+of+overdose%2c+which+can+be+fatal.+However%2c+the+tone+of+this+comment+was+a+little+harsh+for+starting+a+discussion.+%0a%0aAn+expression+of+concern+about+the+risks+involved+would+be+more+effective+at+creating+a+partnership+with+Mr.+Wright.+</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                                <Entry2>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>%22I+am+very+concerned+about+the+potential+for+an+overdose+from+taking+your+next+opioid+dose+early.+It+could+also+lead+to+addiction.%22</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>Starting+with+an+expression+of+concern+about+how+Mr.+Wright+is+taking+his+opioid+medication+before+its+time+for+another+dose+will+help+build+a+partnership+with+him.+After+making+this+connection%2c+he+may+be+more+open+to+hearing+safety+guidelines+and+your+practice%27s+policies.+</FeedbackValue>
                                  </PanelData>
                                </Entry2>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry2>
                        <Entry3>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+didn%27t+think+about+that.+Thanks+for+letting+me+know.+</dialogueText>
                          </PanelData>
                        </Entry3>
                        <Entry4>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I+have+to+be+firm+for+the+sake+of+your+safety+and+require+that+you+to+agree+to+follow+instructions+carefully%2c+if+we+are+to+work+together+on+managing+your+pain.+But+I+will+work+with+you+to+find+the+most+effective+and+safe+combination+of+treatments+possible.+</dialogueText>
                          </PanelData>
                        </Entry4>
                        <Entry5>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>All+right.+</dialogueText>
                          </PanelData>
                        </Entry5>
                        <Entry6>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>What+else+have+you+tried+recently+to+treat+the+pain%3f+</dialogueText>
                          </PanelData>
                        </Entry6>
                        <Entry7>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+also+take+Cymbalta%2c+the+antidepressant%2c+which+they+say+is+supposed+to+help+with+pain%2c+too.+It+might+help+a+little.+I+don%27t+like+to+use+a+cane%2c+because+it+makes+me+look+older.+I+have+two+young+children+and+I+don%27t+want+to+have+them+see+me+as+weak.+I%27d+love+to+be+more+active+with+them.</dialogueText>
                          </PanelData>
                        </Entry7>
                        <Entry8>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <data>
                              <EntryData>
                                <Parent>HistorySection/Pain HxTab/LabEntry: 0::LabEntry: 8</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Keep+going+with+history+questions.+There%e2%80%99s+a+lot+of+information+needed+about+his+pain.+</OptionValue>
                                    <OptionTypeValue>Partially+Correct</OptionTypeValue>
                                    <FeedbackValue>While+it+is+true+that+sometimes+you+need+to+keep+going+when+obtaining+a+history+due+to+time+limitations+and+high+medical+priorities%2c+it+is+also+important+to+acknowledge+deep+concerns+of+the+patient%27s.+You+need+to+show+that+you+are+listening+at+this+level.+It+can+be+done+briefly.+</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Pause+to+offer+some+empathy.+</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>It+is+important+to+both+get+a+complete+history+and+offer+empathy+at+key+points+in+the+interview.+Offering+empathy+can+be+done+briefly.+Even+a+short+pause%2c+eye+contact%2c+and+a+sympathetic+sound+can+make+a+patient+feel+their+most+important+concerns+are+heard.+</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry8>
                        <Entry9>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>%5bPauses.+Makes+eye+contact+and+speaks+with+a+sympathetic+tone.%5d+I+understand+what+you%27re+saying.+What+about+in+the+past%3f+What+has+been+tried+to+treat+your+pain%3f</dialogueText>
                          </PanelData>
                        </Entry9>
                        <Entry10>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Let%27s+see.+I+had+physical+therapy+a+long+time+ago+after+surgery%2c+but+I+it+was+painful+so+I+stopped.+They+did+another%2c+smaller+surgery+a+few+years+later+and+that+helped+a+little.+I%27ve+had+steroid+injections+which+helped+for+a+little+while.+Lortab+and+Percocet+helped+but+the+doctors+got+nervous+about+continuing+to+prescribe+opioids.+I+also+had+Toradol+and+Darvocet%2c+which+were+stopped+because+they+didn%27t+work+well+enough.+%0a%0aI+was+wondering+if+I+need+a+stronger+dose%3f+That+way+I+wouldn%27t+be+tempted+to+take+my+medication+early.</dialogueText>
                          </PanelData>
                        </Entry10>
                        <Entry11>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <data>
                              <EntryData>
                                <Parent>HistorySection/Pain HxTab/LabEntry: 0::LabEntry: 11</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>The+best+solution+is+a+combination+of+pain+treatments%2c+not+increasing+the+opioid+dose.</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>Current+guidelines+for+chronic+pain+management+recommend+using+a+combination+of+pain+treatments+and+limiting+opioid+use+as+much+as+possible.+</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Increase+the+opioid+dose+a+little</OptionValue>
                                    <OptionTypeValue>Incorrect</OptionTypeValue>
                                    <FeedbackValue>Increasing+the+opioid+dose+should+be+the+last+response.+First%2c+all+other+combination+of+chronic+pain+management+should+be+tried.+</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry11>
                        <Entry12>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Often+the+best+solution+for+chronic+pain+is+a+combination+of+pain+treatments%2c+medications+as+well+as+other+treatments%2c+rather+than+just+raising+the+dose+of+opioids.+</dialogueText>
                          </PanelData>
                        </Entry12>
                        <Entry13>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Is+that+right%3f</dialogueText>
                          </PanelData>
                        </Entry13>
                        <Entry14>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>%5bNods%5d+For+me+to+think+about+what+medications+would+be+best%2c+I%27d+like+to+understand+your+risk+of+overdose.+I+need+to+know+about+other+medications+or+drugs+you+take%2c+including+those+prescribed+or+not+prescribed+for+you%2c+particularly%e2%80%a6.+</dialogueText>
                          </PanelData>
                        </Entry14>
                        <Entry15>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <data>
                              <EntryData>
                                <Parent>HistorySection/Pain HxTab/LabEntry: 0::LabEntry: 15</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Benzodiazepines</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>Benzodiazepines+are+potentially+very+dangerous+in+combination+with+opioids.+The+combination+may+lead+to+respiratory+distress+and+potentially+a+fatal+overdose.+A+high+level+of+caution+should+be+employed+with+combining+opioids+with+them+or+other+medications+that+cause+drowsiness+or+relaxation.</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Cocaine</OptionValue>
                                    <OptionTypeValue>Partially+Correct</OptionTypeValue>
                                    <FeedbackValue>Drug+interactions+between+opioids+and+cocaine+are+not+particularly+dangerous.+However%2c+it+is+worth+noting+that+individuals+who+use+illegal+drugs%2c+on+average%2c+are+at+higher+risk+for+opioid+abuse.+</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry15>
                        <Entry16>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>...particularly+benzodiazepines%2c+such+as+Xanax+or+Valium%2c+or+other+medications+that+make+you+drowsy+or+relaxed%3f</dialogueText>
                          </PanelData>
                        </Entry16>
                        <Entry17>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>No%2c+just+oxycodone+and+my+antidepressant.</dialogueText>
                          </PanelData>
                        </Entry17>
                        <Entry18>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Thank+you+for+answering+my+questions+and+helping+me+understand+what+you%27re+experiencing.+I+have+a+couple+of+quick+surveys+that+will+help+me+in+coming+up+with+a+plan+to+manage+your+pain.</dialogueText>
                          </PanelData>
                        </Entry18>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <CustomContentValue>%3cb%3eInstructions%3a%3c%2fb%3e+In+order+to+obtain+a+complete+history+of+Mr.+Wright%27s+pain+and+its+treatment%2c+choose+what+the+provider+should+say+in+the+following+dialogue.+</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <Differential_DiagnosisTab>
        <customTabName>Differential Dx</customTabName>
        <data>
          <EntryData>
            <Parent>HistorySection/Differential DxTab/</Parent>
            <Entry0>
              <PanelType>SymptomClusterEntryPanel</PanelType>
              <PanelData>
                <PriorityValue>Primary+Diagnosis</PriorityValue>
                <SymptomClusterValue>Aberrant+drug-related+behavior+(taking+medication+without+a+prescription)</SymptomClusterValue>
                <data>
                  <EntryData>
                    <Parent>HistorySection/Differential DxTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>DiagnosisEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Under-treated+chronic+pain</DiagnosisValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>DiagnosisEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Opioid+use+disorder</DiagnosisValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>DiagnosisEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Diversion</DiagnosisValue>
                      </PanelData>
                    </Entry2>
                    <Entry3>
                      <PanelType>DiagnosisEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Cognitive+impairment</DiagnosisValue>
                      </PanelData>
                    </Entry3>
                  </EntryData>
                </data>
                <FeedbackValue>1.+Mr.+Wright+explains+that+he+still+has+pain+and+that+this+is+the+reason+he+took+the+medication+early%2c+so+this+explanation+makes+sense+at+face+value.+However%2c+the+other+diagnoses+should+also+be+considered.+%0a%0a2.+Not+following+his+prescription%27s+instructions+is+worrisome+behavior+and+is+one+risk+factor+for+possible+opioid+use+disorder.+Other+risks+should+be+explored.+%0a%0a3.+When+a+patient+needs+more+medication+than+prescribed%2c+even+if+they+have+a+plausible+explanation%2c+diversion+should+be+considered+as+a+possible+reason.+%0a%0a4.+A+cognitive+impairment+could+lead+to+a+patient+taking+his+medication+earlier+than+scheduled%2c+however%2c+Mr.+Wright+has+already+explained+that+he+did+it+intentionally.+Also%2c+he+has+no+other+signs+of+cognitive+impairment.+</FeedbackValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </Differential_DiagnosisTab>
    </_HistorySection>
    <_EvaluationSection>
      <sectionName>
      </sectionName>
      <TextboxTab>
        <customTabName>Evaluation Info</customTabName>
        <data>
          <EntryData>
            <Parent>EvaluationSection/Evaluation InfoTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3cb%3e%3ccolor%3d%23F26F45%3eBACKGROUND+INFORMATION+FOR+EVALUATION%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cb%3eEvaluating+Patients+with+Chronic+Pain+%3c%2fb%3e+%0a%0aIt+is+important+to+evaluate+patients+having+pain+for+psychological+or+social+factors+that+could+affect+their+recovery.+%0a%0a%3cb%3eAssessing+Depression+%3c%2fb%3e+%0a%0aIdentifying+and+treating+depression+is+an+important+component+of+pain+management+because+it+can+have+a+direct+effect+on+the+experience+of+pain.+The+interactions+between+pain+and+depression+are+complex+with+causality+going+in+both+directions%3a+Pain+contributes+to+depression%2c+and+depression+contributes+to+pain.+%0a%0aDepression+is+the+most+common+mood+disorder+among+patients+with+opioid+use+disorder.+An+estimated+44%25+to+54%25+of+patients+with+opioid+use+disorder+have+suffered+from+major+depression+at+some+point+in+their+lifetime+(Pani+et+al.%2c+2010).+Up+to+30%25+of+patients+with+opioid+use+disorder+are+currently+depressed.+%0a%0aHigher+dose+opioids+are+associated+with+increased+symptoms+of+depression.+However%2c+increased+symptoms+of+depression+may+be+due+to+the+high+dose+opioids+or+greater+pain+that+resulted+in+high+dose+opioids+being+prescribed+(Merril+et+al.%2c+2012).+Treatment+for+depression%2c+for+example%2c+cognitive+behavioral+therapy+(CBT)%2c++often+improves+pain.+CBT+may+include+learning+pain+coping+strategies.+A+patient%27s+affect+and+mood+may+also+benefit+from+relaxation+strategies+and+biofeedback.+Traditional+antidepressant+therapy+also+may+help+(Briley+%26+Moret%2c+2008).+%0a%0a%3cb%3eAssessing+Physical+and+Psychosocial+Functioning+%3c%2fb%3e+%0a%0d%0aFunctioning+is+the+impact+of+the+pain+on+the+patient%27s+life.+Include+these+areas+in+your+questions%3a%0d%0a%0d%0a+%e2%80%a2++Psychological+Functioning%2fMood%3a+Does+the+pain+affect+your+mood+or+ability+to+enjoy+life%3f+Note%3a+May+need+to+involve+caregivers+in+this+discussion.%0d%0a%0d%0a+%e2%80%a2+Daily+Activities%3a+Does+your+pain+keep+you+from+doing+anything%2c+such+as+daily+activities%3f+(e.g.%2c+sleeping%2c+walking%2c+cleaning%2c+shopping%2c+work%2c+play%2c+personal+hygiene%2c+childcare%2c+or+hobbies).%0d%0a%0d%0a+%e2%80%a2+Social+Functioning%3a+Does+the+pain+affect+your+relationships%3f%0d%0a(Marshall%2c+2010)%0d%0a%0d%0aAnother+assessment+relevant+to+pain+management+is+the+PEG+scale%2c+which+is+a+quick+assessment+of+pain%2c+functioning%2c+and+impact+of+pain+on+the+patient%27s+life+(Krebs+et+al.%2c+2009).%0d%0a%0a%3cb%3eScreening+for+Opioid+Risk+%3c%2fb%3e+%0aScreening+surveys+may+help+evaluate+a+patient+for+their+risk+if+prescribed+opioids.+The+biggest+risk+factor+is+having+a+history+of+substance+abuse.+Example+screening+survey%3a+%0d%0a%0d%0a%3cb%3eOpioid+Risk+Tool+(ORT)+%3c%2fb%3e+(Webster%2c+2005)%0d%0aQuestions+cover%3a%0d%0a1.+Family+history+of+substance+abuse%0d%0a2.+Personal+history+of+substance+abuse%0d%0a3.+Age+(16-45+has+the+highest+risk)%0d%0a4.+History+of+preadolescent+sexual+abuse+(for+females+only)%0d%0a5.+Psychological+Disease%0d%0aAlternative+screening+survey%3a+Diagnosis%2c+Intractability%2c+Risk%2c+Efficacy+(DIRE)+(Belgrade%2c+2006)%0d%0a%0a%3cb%3ePrescription+Drug+Monitoring%3c%2fb%3e%0aConsult+prescription+drug+monitoring+program+database+before+prescribing+opioids+and+during+treatment%3a+Look+at+total+opioid+doses+and+dangerous+drug+combinations.+Check+database+at+least+every+3+months+and+consider+checking+at+every+prescription.%0d%0a%0aRecommendations+from%3a+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016+(Dowell%2c+et+al.%2c+2016)%0a</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <Screen_AssessTab>
        <customTabName>Substance Use</customTabName>
        <data>
          <EntryData>
            <Parent>EvaluationSection/Substance UseTab/</Parent>
            <Entry0>
              <PanelType>ScreenAssessEntryPanel</PanelType>
              <PanelData>
                <SurveyTitleValue>CAGE-AID</SurveyTitleValue>
                <DialoguePin>
                  <dialogue>
                    <uid>EvaluationSection/Substance UseTab/LabEntry: 0</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>EvaluationSection/Substance UseTab/LabEntry: 0</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I%27m+interested+in+learning+more+about+your+responses+to+these+surveys.+For+example%2c+you+said+you+feel+you+should+cut+back+on+your+alcohol+use%2c+too.+Can+you+say+more+about+that%3f++</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Sure.+I+don%27t+drink+much+-+just+1+or+2+beers+a+few+times%2c+maybe+4+times+a+week+at+the+most%2c+but+the+doctor+who+prescribed+my+antidepressant+said+I+shouldn%27t+drink+alcohol+while+I+take+it+or+it+could+hurt+my+liver.+And+the+other+doctor+said+that+I%27m+not+supposed+to+mix+alcohol+and+opioids.+But+I+sometimes+drink+anyhow+when+I%27m+feeling+a+little+depressed+or+my+pain+is+bad.</dialogueText>
                          </PanelData>
                        </Entry1>
                        <Entry2>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Yes%2c+alcohol+can+have+harmful+effects+together+with+your+antidepressant+and+using+alcohol+with+opioids+increases+your+risk+of+overdose+and+problems+driving+or+operating+machinery.+%0a%0aWe%27ll+need+to+maximize+your+pain+control%2c+but+it+sounds+like+it+would+also+help+to+get+some+help+with+more+pain+coping+skills+and+remaining+depression%2c+so+that+you+won%27t+want+to+turn+to+drinking.</dialogueText>
                          </PanelData>
                        </Entry2>
                        <Entry3>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>That+sounds+good.</dialogueText>
                          </PanelData>
                        </Entry3>
                        <Entry4>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>You+say+you+sometimes+take+something+first+thing+in+the+morning+to+steady+your+nerves.+Is+that+alcohol+or+oxycodone%3f</dialogueText>
                          </PanelData>
                        </Entry4>
                        <Entry5>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Just+oxycodone.+If+I+don%27t+take+my+it%2c+I+get+shaky+by+mid-morning.+That%27s+withdrawal%2c+I+guess.</dialogueText>
                          </PanelData>
                        </Entry5>
                        <Entry6>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <data>
                              <EntryData>
                                <Parent>EvaluationSection/Substance UseTab/LabEntry: 0::LabEntry: 6</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Yes%2c+that+sounds+like+withdrawal.</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>Yes%2c+feeling+%e2%80%9cshaky%e2%80%9d+by+mid-morning+after+not+having+an+opioid+dose+since+the+previous+day+is+a+symptom+of+opioid+withdrawal.+Other+symptoms+of+opioid+withdrawal+are%3a+%0a%c2%a0%e2%80%a2%c2%a0+Dysphoric+mood%0d%0a%c2%a0%e2%80%a2%c2%a0+Lacrimation+or+rhinorrhea%0d%0a%c2%a0%e2%80%a2%c2%a0+Fever</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>No%2c+that%27s+not+withdrawal.</OptionValue>
                                    <OptionTypeValue>Incorrect</OptionTypeValue>
                                    <FeedbackValue>Feeling+%e2%80%9cshaky%e2%80%9d+by+mid-morning+after+not+having+an+opioid+dose+since+the+previous+day+is+a+symptom+of+opioid+withdrawal.+</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry6>
                        <Entry7>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>It+sounds+like+it+could+be+withdrawal.+People+who+take+opioids+for+chronic+pain+do+become+physically+dependent+on+them+and+get+withdrawal+symptoms+when+they+go+for+a+while+without+any+opioids.+</dialogueText>
                          </PanelData>
                        </Entry7>
                        <Entry8>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Yes%2c+that%27s+what+happens.+</dialogueText>
                          </PanelData>
                        </Entry8>
                        <Entry9>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>And+you+said+that+you+feel+you+should+cut+down+on+substance+use+and+feel+some+guilt+about+your+use.++Did+you+mean+opioids%2c+alcohol%2c+or+other+substances%3f+</dialogueText>
                          </PanelData>
                        </Entry9>
                        <Entry10>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+meant+oxycodone%2c+for+one.+I+feel+like+I+need+it+even+when+the+pain+is+better+and+I+don%27t+like+that.+So%2c+I+feel+guilty+and+want+to+cut+back.+But+I%27ve+struggled+with+that.</dialogueText>
                          </PanelData>
                        </Entry10>
                        <Entry11>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I%27d+like+to+work+with+you+to+help+you+with+your+need+for+opioids.+With+today%27s+treatment+guidelines%2c+you+would+not+have+been+kept+on+opioids+for+this+long.+I+can%27t+say+for+sure+about+your+situation+and+sometimes+opioids+are+needed%2c+but+current+treatment+recommendations+are+for+3+days+of+opioids+or+at+the+most%2c+one+week%2c+for+acute+pain.++</dialogueText>
                          </PanelData>
                        </Entry11>
                        <Entry12>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+wish+I+knew+that+back+then%e2%80%a6I+also+said+%22yes%22+to+those+questions+because+I+feel+guilty+about+drinking+alcohol.</dialogueText>
                          </PanelData>
                        </Entry12>
                        <Entry13>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Would+you+consider+going+to+counseling+again%3f+I+could+help+you+find+someone+who+specializes+in+pain+coping+skills+and+helping+with+depression.+</dialogueText>
                          </PanelData>
                        </Entry13>
                        <Entry14>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>That+sounds+good.+I+might+be+interested%2c+depending+upon+costs+and+all+that.</dialogueText>
                          </PanelData>
                        </Entry14>
                        <Entry15>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>That+sounds+good.+I+might+be+interested%2c+depending+upon+costs+and+all+that.</dialogueText>
                          </PanelData>
                        </Entry15>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <SurveyInstructionValue>When+thinking+about+drug+use%2c+include+illegal+drug+use+and+the+use+of+prescription+drug+other+than+prescribed.</SurveyInstructionValue>
                <data>
                  <EntryData>
                    <Parent>EvaluationSection/Substance UseTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Have+you+ever+felt+you+ought+to+cut+down+on+your+drinking+or+drug+use%3f</QuestionValue>
                        <AnswerValue>Yes%2c+but+only+opioids+</AnswerValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Have+people+annoyed+you+by+criticizing+your+drinking+or+drug+use%3f</QuestionValue>
                        <AnswerValue>No</AnswerValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Have+you+ever+felt+bad+or+guilty+about+your+drinking+or+drug+use%3f</QuestionValue>
                        <AnswerValue>Yes%2c+but+only+for+taking+extra+opioids</AnswerValue>
                      </PanelData>
                    </Entry2>
                    <Entry3>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Have+you+ever+had+a+drink+or+drug+first+thing+in+the+morning+to+steady+your+nerves+or+to+get+rid+of+a+hangover%3f</QuestionValue>
                        <AnswerValue>Yes</AnswerValue>
                      </PanelData>
                    </Entry3>
                  </EntryData>
                </data>
                <ScoreValue>3+points</ScoreValue>
                <ScoreKeyValue>A+score+of+2+or+more+points+is+positive.+A+score+of+1+point+is+also+positive+if+it+is+for+the+last+question.</ScoreKeyValue>
                <InterpretationValue>Positive+screen.++Further+assessment+is+indicated.</InterpretationValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </Screen_AssessTab>
      <Screen_AssessTab>
        <customTabName>Other Assessments</customTabName>
        <data>
          <EntryData>
            <Parent>EvaluationSection/Other AssessmentsTab/</Parent>
            <Entry0>
              <PanelType>ScreenAssessEntryPanel</PanelType>
              <PanelData>
                <SurveyTitleValue>Beck+Depression+Inventory+(Beck+et+al.%2c+1988)</SurveyTitleValue>
                <SurveyInstructionValue>
                </SurveyInstructionValue>
                <data>
                  <EntryData>
                    <Parent>EvaluationSection/Other AssessmentsTab/LabEntry: 0</Parent>
                  </EntryData>
                </data>
                <ScoreValue>7+points.+This+was+due+to+Mr.+Wright+giving+mild%2c+positive+responses+to+symptoms%3a+sadness%2c+pessimism%2c+guilt%2c+self-dislike%2c+irritability%2c+work+difficulty%2c+and+fatigability.</ScoreValue>
                <ScoreKeyValue>A+score+of+0-13+is+considered+minimal+depression.</ScoreKeyValue>
                <InterpretationValue>Minimal+depression</InterpretationValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>ScreenAssessEntryPanel</PanelType>
              <PanelData>
                <SurveyTitleValue>PEG+Scale+(Krebs+et+al.%2c+2009)+Functioning+Screening</SurveyTitleValue>
                <SurveyInstructionValue>
                </SurveyInstructionValue>
                <data>
                  <EntryData>
                    <Parent>EvaluationSection/Other AssessmentsTab/LabEntry: 1</Parent>
                    <Entry0>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Average+pain%3f</QuestionValue>
                        <AnswerValue>5+out+of+10</AnswerValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Pain+interference+with+enjoyment+of+life%3f</QuestionValue>
                        <AnswerValue>6+out+of+10</AnswerValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Pain+interference+with+general+activity%3f</QuestionValue>
                        <AnswerValue>3+out+of+10</AnswerValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
                <ScoreValue>Mr.+Wright%27s+raw+score+was+14.+Divided+by+3+equals+a+final+score+of+4.7.</ScoreValue>
                <ScoreKeyValue>The+PEG+Scale+is+used+primarily+to+track+changes+in+pain+and+functioning+over+time.</ScoreKeyValue>
                <InterpretationValue>The+total+score+is+divided+by+10.+Mr.+Wright%27s+results+are+his+total+score+of+14+divided+by+3+which+equals+4.7+out+of+a+total+possible+score+of+10.+This+is+his+baseline+Peg+Scale+result+and+should+be+compared+to+future+results.+A+change+for+the+individual+patient+is+a+measure+of+pain+and+functioning+response+to+treatment.+Repeat+at+each+appointment.</InterpretationValue>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </Screen_AssessTab>
      <Physical_ExamTab>
        <customTabName>Phys Ex</customTabName>
        <data>
          <EntryData>
            <Parent>EvaluationSection/Phys ExTab/</Parent>
            <Entry0>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>General+Appearance</PanelNameValue>
                <CollapseButtonRadio>True</CollapseButtonRadio>
                <NotesValue>Well-nourished%2c+hyperactive+(self-report)</NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>HEENT</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Neck</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Musculoskeletal</PanelNameValue>
                <CollapseButtonRadio>True</CollapseButtonRadio>
                <NotesValue>No+swelling+or+redness+of+right+knee+or+any+other+joints.+See+%22Extremities%22+for+a+description+of+knee+functionality+limitations.+</NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry3>
            <Entry4>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Lungs</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry4>
            <Entry5>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Heart</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry5>
            <Entry6>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Neurological%2fPsych</PanelNameValue>
                <CollapseButtonRadio>True</CollapseButtonRadio>
                <NotesValue>Right+knee%3a+%0a%c2%a0%e2%80%a2%c2%a0+Marked+crepitus+throughout+range+of+motion%0a%c2%a0%e2%80%a2%c2%a0+Severe+pain+elicited+by+ascending+and+descending+stairs%0a%c2%a0%e2%80%a2%c2%a0+Mild+to+moderate+pain+with+both+passive+and+active+motions%0a%c2%a0%e2%80%a2%c2%a0+Mild+atrophy+and+weakness+of+quadriceps%0a%c2%a0%e2%80%a2%c2%a0+Ligament+tests+unremarkable%0a%c2%a0%e2%80%a2%c2%a0+Normal+range+of+motion%0aOther+joints+WNL</NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry6>
            <Entry7>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Extremities</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry7>
            <Entry8>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Skin</PanelNameValue>
                <CollapseButtonRadio>True</CollapseButtonRadio>
                <NotesValue>No+rashes+or+infections</NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry8>
            <Entry9>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Rectum</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry9>
            <Entry10>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Abdomen</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry10>
            <Entry11>
              <PanelType>PhysicalExamEntryPanel</PanelType>
              <PanelData>
                <PanelNameValue>Genitourinary</PanelNameValue>
                <WithinNormalLimitsToggle>True</WithinNormalLimitsToggle>
                <CollapseButtonRadio>
                </CollapseButtonRadio>
                <NotesValue>
                </NotesValue>
                <Image>
                </Image>
              </PanelData>
            </Entry11>
          </EntryData>
        </data>
      </Physical_ExamTab>
      <TextboxTab>
        <customTabName>PDMP Report</customTabName>
        <data>
          <EntryData>
            <Parent>EvaluationSection/PDMP ReportTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3cb%3ePrescription+Drug+Monitoring+Report%3c%2fb%3e%0a%0a%3cb%3ePatient%3a%3c%2fb%3e+Chad+Wright%0a%0a%3cb%3eSummary%3a+%3c%2fb%3e%0a+%e2%80%a2+Mr.+Wright+received+a+prescription+for+extended-release+oxycodone+starting+a+little+over+a+year+ago%2c+that+ran+out+yesterday.++%0a+%e2%80%a2+Initially%2c+he+was+prescribed+immediate-release+oxycodone%2c+the+dose+was+gradually+increased%2c+and+then+he+was+switched+to+extended-release+oxycodone.+%0a+%e2%80%a2+Earlier%2c+he+was+prescribed+oxycodone+(10+mg)+plus+acetaminophen+(325+mg)+for+almost+a+year+by+a+different+provider.+%0d%0a+%e2%80%a2+Although+he+has+switched+doctors+a+number+of+times%2c+he+only+sees+one+doctor+at+a+time+and+only+uses+one+pharmacy.+</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
    </_EvaluationSection>
    <_Medical_TestsSection>
      <sectionName>
      </sectionName>
      <TextboxTab>
        <customTabName>Medical Tests Info</customTabName>
        <data>
          <EntryData>
            <Parent>Medical_TestsSection/Medical Tests InfoTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3ccolor%3d%2333CCCC%3e%3cb%3eGUIDELINES+FOR+MEDICAL+TESTS+RELEVANT+TO+THIS+CASE%3c%2fb%3e%3c%2fcolor%3e%3cb%3e%0a%0aFrom+CDC+Guidelines+for+Prescribing+Opioids+for+Chronic+Pain+(Dowell+et+al.%2c+2016%3c%2fb%3e)%0a%0aGuidelines+for+using+opioids+to+treat+chronic+pain+include+the+following+recommendations%3a%0a%3cb%3eUrine+Drug+Testing%3c%2fb%3e%0aUse+urine+drug+testing+before+and+during+opioid+treatment+at+least+annually.+Test+for+the+prescribed+medications%2c+controlled+prescription+drugs%2c+and+illicit+drugs.+Frequency+varies+with+individual+clinician%2fclinic+and%2for+patient+situation.%0d%0a%0a%3cb%3ePrescription+Drug+Monitoring%3c%2fb%3e%0aConsult+prescription+drug+monitoring+program+database+before+prescribing+opioids+and+during+treatment%3a+Look+at+total+opioid+doses+and+dangerous+drug+combinations.+Check+database+at+least+every+3+months+and+consider+checking+at+every+prescription.%0d%0a%0aRecommendations+from%3a+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016+(Dowell%2c+et+al.%2c+2016)%0d%0a%0a%3cb%3eImaging+and+Osteoarthritis+of+the+Knee%3c%2fb%3e%0d%0aRadiographic+changes+that+might+indicate+osteoarthritis+in+the+knee+include+the+presence+of+osteophytes%2c+which+are+an+overgrowth+of+bone+in+response+to+the+chronic+inflammation%2c+and+joint+space+narrowing+(Kellgren+%26+Lawrence%2c+2000)%0a</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <Medical_TestsTab>
        <customTabName>Radiology</customTabName>
        <data>
          <EntryData>
            <Parent>Medical_TestsSection/RadiologyTab/</Parent>
            <Entry0>
              <PanelType>Other Medical Tests</PanelType>
              <PanelData>
                <TestValue>MRI</TestValue>
                <DetailsValue>Minor+calcifications+in+the+joint+space+near+ACL+insertion%2c+right+knee</DetailsValue>
                <InterpretationValue>Patellofemoral+and+tibiofemoral+osteoarthritis%3a+possible+osteophyte+lipping+and+possible+joint+space+narrowing+on+anteroposterior+view%2c+right+knee.</InterpretationValue>
                <Image>0fb8b554cf</Image>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>It+is+important+to+evaluate+the+current+status+of+pain+conditions+periodically+in+patients+on+chronic+opioid+therapy+to+help+determine+if+opioids+are+still+indicated.+His+last+MRI+was+16+years+ago%2c+so+another+one+was+indicated.</FeedbackValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>Other Medical Tests</PanelType>
              <PanelData>
                <TestValue>No+imaging+indicated+at+this+time</TestValue>
                <DetailsValue>No+imaging+results+with+this+choice</DetailsValue>
                <InterpretationValue>
                </InterpretationValue>
                <Image>
                </Image>
                <OptionTypeValue>Incorrect</OptionTypeValue>
                <FeedbackValue>It+is+important+to+evaluate+the+current+status+of+pain+conditions+periodically+in+patients+on+chronic+opioid+therapy+to+help+determine+if+opioids+are+still+indicated.+His+last+MRI+was+16+years+ago%2c+so+another+one+was+indicated.+</FeedbackValue>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </Medical_TestsTab>
      <Medical_TestsTab>
        <customTabName>Lab Tests</customTabName>
        <data>
          <EntryData>
            <Parent>Medical_TestsSection/Lab TestsTab/</Parent>
            <Entry0>
              <PanelType>Single Lab Test</PanelType>
              <PanelData>
                <TestNameValue>Urine+Toxicology</TestNameValue>
                <NormalRangeValue>
                </NormalRangeValue>
                <WNLToggle>True</WNLToggle>
                <ValueValue>Positive+for+some+drugs+tested</ValueValue>
                <Image>
                </Image>
                <InterpretationValue>Positive+for+oxycodone%2c+oxymorphone.+Clear+for+all+other+drugs+tested+(marijuana+metabolites%2c+cocaine+metabolites%2c+other+opioids+and+metabolites%2c+phencyclidine%2c+amphetamines).</InterpretationValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Urine+drug+tests+are+indicated+before+prescribing+opioids+and+periodically+during+chronic+opioid+therapy.+Mr.+Wright%e2%80%99s+urine+drug+test+results+are+what+would+be+expected+for+someone+taking+oxycodone%2c+that+is%2c+they+include+oxycodone+and+a+metabolite.+</FeedbackValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>Single Lab Test</PanelType>
              <PanelData>
                <TestNameValue>Routine+urinalysis</TestNameValue>
                <NormalRangeValue>
                </NormalRangeValue>
                <WNLToggle>True</WNLToggle>
                <ValueValue>None</ValueValue>
                <Image>
                </Image>
                <InterpretationValue>Test+not+completed%3b+no+medical+indication+for+it+in+this+case.</InterpretationValue>
                <OptionTypeValue>Partially+Correct</OptionTypeValue>
                <FeedbackValue>Urinalysis+could+be+indicated+depending+upon+the+patient%27s+medical+status+and+need+for+general+physical+evaluation.+Mr.+Wright+had+no+indications+of+other+medical+problems.</FeedbackValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>Single Lab Test</PanelType>
              <PanelData>
                <TestNameValue>No+lab+tests+indicated</TestNameValue>
                <NormalRangeValue>
                </NormalRangeValue>
                <WNLToggle>True</WNLToggle>
                <ValueValue>None</ValueValue>
                <Image>
                </Image>
                <InterpretationValue>
                </InterpretationValue>
                <OptionTypeValue>Incorrect</OptionTypeValue>
                <FeedbackValue>Urine+drug+tests+are+indicated+before+prescribing+opioids+and+periodically+during+chronic+opioid+therapy.+Mr.+Wright%e2%80%99s+urine+drug+test+results+are+what+would+be+expected+for+someone+taking+oxycodone%2c+that+is%2c+they+include+oxycodone+and+a+metabolite.+</FeedbackValue>
              </PanelData>
            </Entry2>
          </EntryData>
        </data>
      </Medical_TestsTab>
    </_Medical_TestsSection>
    <_DiagnosisSection>
      <sectionName>
      </sectionName>
      <TextboxTab>
        <customTabName>Diagnosis Info</customTabName>
        <data>
          <EntryData>
            <Parent>DiagnosisSection/Diagnosis InfoTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3cb%3e%3ccolor%3d%23EB4D5C%3eBACKGROUND+INFORMATION+FOR+DIAGNOSIS%3c%2fcolor%3e%3c%2fb%3e%0a%3cb%3eDiagnostic+Criteria+for+Opioid+Use+Disorder%3c%2fb%3e%0a%0aA+diagnosis++of+Opioid+Use+Disorder%2c+according+to+the+DSM-5%2c+requires+a+pattern+of+using+opioids+causing+clinically+significant+impairment+or+distress+and+meeting+at+least+2+of+the+following+criteria%3a%0a%0a+%e2%80%a2+Taking+the+opioid+in+larger+amounts+and+for+longer+than+intended%0a+%e2%80%a2+Wanting+to+cut+down+or+quit+but+not+being+able+to+do+it%0a+%e2%80%a2+Spending+a+lot+of+time+obtaining+the+opioid+%0a+%e2%80%a2+Craving+or+a+strong+desire+to+use+opioids%0a+%e2%80%a2+Repeatedly+being+unable+to+carry+out+major+obligations+at+work%2c+school%2c+or+home+due+to+opioid+use%0a+%e2%80%a2+Continuing+use+despite+it+causing+persistent+or+recurring+social+or+interpersonal+problems%0a+%e2%80%a2+Stopping+or+reducing+important+social%2c+occupational%2c+or+recreational+activities+due+to+opioid+use%0a+%e2%80%a2+Recurrently+using+opioids+in+physically+hazardous+situations%0a+%e2%80%a2+Consistently+using+opioids+despite+it+causing+persistent+or+recurrent+physical+or+psychological+difficulties%0a+%e2%80%a2+Being+tolerant+for+opioids+(needing+increased+amounts+to+achieve+the+desired+effect+or+experiencing+diminished+effect+with+the+same+amount)%0a+%e2%80%a2+Experiencing+withdrawal+or+using+the+substance+to+avoid+withdrawal.+The+criteria+of+tolerance+and+withdrawal+do+not+apply+when+the+opioid+is+being+used+appropriately+under+medical+supervision.%0a%0aThe+above+criteria+are+paraphrased+from+the+DSM+5+(PCSS+reproduction+of+APA%2c+2013)</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <QuizTab>
        <customTabName>Diagnosis Rounds</customTabName>
        <data>
          <EntryData>
            <Parent>DiagnosisSection/Diagnosis RoundsTab/</Parent>
            <Entry0>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>Mr.+Wright+is+being+treated+for+both+chronic+pain+and+depression.+%0a%0aChoose+the+best+description+for+the+interaction+between+chronic+depression+and+chronic+pain%3f+(Choose+all+that+apply)</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Checkboxes</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>DiagnosisSection/Diagnosis RoundsTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Chronic+depression+contributes+to+chronic+pain</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Chronic+depression+contributes+to+chronic+pain+and+vice+versa.</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Chronic+pain+contributes+to+depression</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Chronic+depression+contributes+to+chronic+pain+and+vice+versa.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Neither+statement+above+is+true</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Both+statements+are+true+in+a+complex+interaction%3a+Chronic+depression+contributes+to+chronic+pain+and+chronic+pain+contributes+to+depression.</FeedbackValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>True+or+False.+A+diagnosis+of+Opioid+Use+Disorder+should+be+reserved+for+individuals+having+a+pattern+of+opioid+use+causing+%22clinically+significant+impairment+or+distress%22+and+meeting+5+or+more+diagnostic+criteria.</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>DiagnosisSection/Diagnosis RoundsTab/LabEntry: 1</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>True</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Instead+of+5+criteria%2c+only+2+or+more+diagnostic+criteria+are+required+for+this+diagnosis.+The+diagnosis+is+further+specified+according+to+how+many+criteria+are+met.+Having+2-3+criteria+met+is+considered+mild+opioid+use+disorder%2c+4-5+criteria+is+moderate%2c+and+6+or+more+criteria+is+severe.</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>False</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Instead+of+5+criteria%2c+only+2+or+more+diagnostic+criteria+are+required+for+this+diagnosis.+The+diagnosis+is+further+specified+according+to+how+many+criteria+are+met.+Having+2-3+criteria+met+is+considered+mild+opioid+use+disorder%2c+4-5+criteria+is+moderate%2c+and+6+or+more+criteria+is+severe.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                  </EntryData>
                </data>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </QuizTab>
      <DiagnosisTab>
        <customTabName>Diagnosis</customTabName>
        <data>
          <EntryData>
            <Parent>DiagnosisSection/DiagnosisTab/</Parent>
            <Entry0>
              <PanelType>DiagnosisEntryPanel</PanelType>
              <PanelData>
                <PriorityValue>Primary+Diagnosis</PriorityValue>
                <SymptomClusterValue>Chronic+pain+and+limited+functioning%2c+right+knee</SymptomClusterValue>
                <data>
                  <EntryData>
                    <Parent>DiagnosisSection/DiagnosisTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>DxEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Crystalline+joint+disease%2c+right+knee</DiagnosisValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>This+diagnosis+should+be+included+in+the+differential%2c+however%2c+it+seems+unlikely.+Crystalline+joint+disease%2c+such+as+gout+would+typically+have+an+acute+onset%2c+present+with+redness+and+swelling%2c+and+affect+multiple+other+joints+as+would+many+other+forms+of+arthritis.+Gout+also+would+often+have+a+history+of+toe+involvement+which+is+missing+in+this+case.</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>DxEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Osteoarthritis+and+joint+fibrosis%2c+right+knee</DiagnosisValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>This+diagnosis+was+confirmed+by+recent+MRI+and+is+supported+by+symptoms+and+history.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>DiagnosisEntryPanel</PanelType>
              <PanelData>
                <PriorityValue>Secondary+Diagnosis</PriorityValue>
                <SymptomClusterValue>Opioid+tolerance%2c+withdrawal%2c+positive+CAGE-AID+screening+for+opioid+use%2c+aberrant+drug-related+behavior+(taking+medication+without+a+prescription)%2c+and+several+risk+factors+for+substance+use+problems.</SymptomClusterValue>
                <data>
                  <EntryData>
                    <Parent>DiagnosisSection/DiagnosisTab/LabEntry: 1</Parent>
                    <Entry0>
                      <PanelType>DxEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Opioid+use+problem+needing+further+evaluation</DiagnosisValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Mr.+Wright+needs+further+evaluation+because+it%27s+not+clear+whether+he+meets+the+description+and+criteria+for+the+diagnosis+of+opioid+use+disorder.+%0d%0a%0d%0aThe+description+for+%e2%80%9copioid+use+disorder%e2%80%9d+is+%e2%80%9ca+pattern+of+using+opioids+causing+clinically+significant+impairment+or+distress.%e2%80%9d+Mr.+Wright+would+also+have+to+meet+at+least+two+of+the+diagnostic+criteria+to+have+this+diagnosis.+He+currently+meets+these+diagnostic+criterion%3a+wanting+to+cut+down+or+quit+but+not+being+able+to%2c+withdrawal+symptoms+when+he+does+not+take+opioids%2c+and+tolerance+(needing+increased+amount+of+the+medication+for+the+same+effect)+but+the+last+two+criteria+are+not+counted+for+a+person+being+treated+for+pain+with+chronic+opioid+therapy.+%0dSee+Diagnosis+Information+for+a+full+list+of+the+diagnostic+criteria.+%0a%0d%0aFurthermore%2c+his+behavior+might+be+a+response+to+unmanaged+pain.+His+pain+should+be+managed+and+then+the+possibility+of+opioid+use+disorder+can+be+re-visited.+%0d%0a</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>DxEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Unmanaged+pain%2c+no+opioid+use+disorder</DiagnosisValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>It+is+possible+that+his+behavior+could+be+explained+entirely+by+a+response+to+unmanaged+pain.+Once+his+pain+is+better+managed%2c+the+possibility+of+opioid+use+disorder+can+be+re-assessed.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>DxEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Opioid+use+disorder</DiagnosisValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Mr.+Wright+needs+further+evaluation+for+the+diagnosis+of+opioid+use+disorder.++%0d%0aIt+is+not+clear+whether+he+meets+the+description+for+opioid+use+disorder%2c+which+is%3a+%0a%0a%22A+pattern+of+using+opioids+causing+%22clinically+significant+impairment+or+distress.%22+Also%2c+to+have+this+diagnosis%2c+he+would+have+to+meet+at+least+two+of+the+diagnostic+criteria.+He+currently+meets+one+diagnostic+criterion%2c+%22Wanting+to+cut+down+or+quit+but+not+being+able+to+do+it.%e2%80%9d+He+does+experience+two+other+criteria%3a+withdrawal+symptoms+when+he+does+not+take+opioids+and+tolerance+(needing+increased+amount+of+the+medication+for+the+same+effect)%2c+but+these+two+criteria+are+not+counted+for+a+person+being+treated+for+pain+with+chronic+opioid+therapy.+See+Diagnosis+Information+for+a+full+list+of+the+diagnostic+criteria.</FeedbackValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>DiagnosisEntryPanel</PanelType>
              <PanelData>
                <PriorityValue>Secondary+Diagnosis</PriorityValue>
                <SymptomClusterValue>Constipation</SymptomClusterValue>
                <data>
                  <EntryData>
                    <Parent>DiagnosisSection/DiagnosisTab/LabEntry: 2</Parent>
                    <Entry0>
                      <PanelType>DxEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Constipation+from+inadequate+dietary+fiber</DiagnosisValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>It+is+possible+that+inadequate+dietary+fiber+is+contributing+to+this+symptom.+Many+people+in+the+U.S.+get+inadequate+fiber+in+their+diet.++However%2c+there+is+a+more+likely+primary+cause+for+his+constipation.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>DxEntry</PanelType>
                      <PanelData>
                        <DiagnosisValue>Opioid-Induced+Constipation</DiagnosisValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Constipation+is+one+of+the+most+common+side+effects+of+chronic+opioid+use.+The+patient+reported+a+connection+between+when+he+was+on+opioids+and+when+he+had+constipation%2c+so+the+diagnosis+seems+likely.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                  </EntryData>
                </data>
              </PanelData>
            </Entry2>
          </EntryData>
        </data>
      </DiagnosisTab>
    </_DiagnosisSection>
    <_Treatment_PlanningSection>
      <sectionName>
      </sectionName>
      <TextboxTab>
        <customTabName>Tx Planning Info</customTabName>
        <data>
          <EntryData>
            <Parent>Treatment_PlanningSection/Tx Planning InfoTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3cb%3e%3ccolor%3d%23FF0000%3eBACKGROUND+INFORMATION+FOR+OPIOID+RISK+SCREENING%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cB%3eOpioid+Risk+Tool+(ORT)+%3c%2fb%3e+(Webster%2c+2005)%0d%0aQuestions+cover%3a%0d%0a1.+Family+history+of+substance+abuse%0d%0a2.+Personal+history+of+substance+abuse%0d%0a3.+Age+(16-45+has+the+highest+risk)%0d%0a4.+History+of+preadolescent+sexual+abuse+(for+females+only)%0d%0a5.+Psychological+Disease%0d%0aAlternative+screening+survey%3a+Diagnosis%2c+Intractability%2c+Risk%2c+Efficacy+(DIRE)+(Belgrade%2c+2006)%0d%0a%3cb%3ePrescription+Drug+Monitoring%3c%2fb%3e%0aConsult+prescription+drug+monitoring+program+database+before+prescribing+opioids+and+during+treatment%3a+Look+at+total+opioid+doses+and+dangerous+drug+combinations.+Check+database+at+least+every+3+months+and+consider+checking+at+every+prescription.%0d%0a%0aRecommendations+from%3a+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016+(Dowell%2c+et+al.%2c+2016)</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <Screen_AssessTab>
        <customTabName>Assess Opioid Risk</customTabName>
        <data>
          <EntryData>
            <Parent>Treatment_PlanningSection/Assess Opioid RiskTab/</Parent>
            <Entry0>
              <PanelType>ScreenAssessEntryPanel</PanelType>
              <PanelData>
                <SurveyTitleValue>Opioid+Risk+Tool+(ORT)+(Webster%2c+2005)</SurveyTitleValue>
                <QuizPin>
                  <data>
                    <EntryData>
                      <Parent>Treatment_PlanningSection/Assess Opioid RiskTab/LabEntry: 0</Parent>
                      <Entry0>
                        <PanelType>QuizQuestion</PanelType>
                        <PanelData>
                          <QuestionValue>Please+interpret+Mr.+Wright%27s+Opioid+Risk+Tool+Score.+</QuestionValue>
                          <Image>
                          </Image>
                          <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                          <data>
                            <EntryData>
                              <Parent>Treatment_PlanningSection/Assess Opioid RiskTab/LabEntry: 0::LabEntry: 0</Parent>
                              <Entry0>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>Low</OptionValue>
                                  <OptionTypeValue>Incorrect</OptionTypeValue>
                                  <FeedbackValue>Mr.+Wright+scored+between+7+and+11+points+on+the+Opioid+Risk+Tool.+Low+risk+is+0-3+points.</FeedbackValue>
                                </PanelData>
                              </Entry0>
                              <Entry1>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>Moderate</OptionValue>
                                  <OptionTypeValue>Incorrect</OptionTypeValue>
                                  <FeedbackValue>Mr.+Wright+scored+between+7+and+11+points+on+the+Opioid+Risk+Tool.+Moderate+risk+scores+are+4+to+7+points.+</FeedbackValue>
                                </PanelData>
                              </Entry1>
                              <Entry2>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>High</OptionValue>
                                  <OptionTypeValue>Correct</OptionTypeValue>
                                  <FeedbackValue>Mr.+Wright+scored+between+7+and+11+points+on+the+Opioid+Risk+Tool.+High+Risk+scores+are+7+or+more+points.+</FeedbackValue>
                                </PanelData>
                              </Entry2>
                            </EntryData>
                          </data>
                        </PanelData>
                      </Entry0>
                    </EntryData>
                  </data>
                </QuizPin>
                <SurveyInstructionValue>
                </SurveyInstructionValue>
                <data>
                  <EntryData>
                    <Parent>Treatment_PlanningSection/Assess Opioid RiskTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Family+History+of+Substance+Abuse+of+Alcohol%2fDrugs</QuestionValue>
                        <AnswerValue>Yes+%e2%80%93+Father+alcohol+(score+for+males+4+points)</AnswerValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Personal+History+of+Substance+Abuse+%e2%80%93+Alcohol</QuestionValue>
                        <AnswerValue>No</AnswerValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Personal+History+of+Substance+Abuse+%e2%80%93+Illegal+Drugs+</QuestionValue>
                        <AnswerValue>No</AnswerValue>
                      </PanelData>
                    </Entry2>
                    <Entry3>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Personal+History+of+Substance+Abuse+%e2%80%93+Prescription+Drugs</QuestionValue>
                        <AnswerValue>No*+However%2c+this+might+be+interpreted+as+a+score+of+5+points%2c+because%2c+although+Mr.+Wright+answers+no%2c+the+answer+is+actually+%22yes%22+according+to+other+history.+</AnswerValue>
                      </PanelData>
                    </Entry3>
                    <Entry4>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Age+16-45</QuestionValue>
                        <AnswerValue>Yes+(score+1+point)</AnswerValue>
                      </PanelData>
                    </Entry4>
                    <Entry5>
                      <PanelType>SurveyQuestionEntry</PanelType>
                      <PanelData>
                        <QuestionValue>Psychological+Disease+%e2%80%93+Attention+Deficit+Hyperactivity+Disorder%2c+Obsessive+Compulsive+Disorder%2c+Bipolar+Disorder%2c+Schizophrenia</QuestionValue>
                        <AnswerValue>Yes%2c+mild+ADHD+(score+1+point)</AnswerValue>
                      </PanelData>
                    </Entry5>
                  </EntryData>
                </data>
                <ScoreValue>11+points+(7+points%2c+if+you+don%27t+count+his+problem+with+taking+prescribed+opioids+too+early)</ScoreValue>
                <ScoreKeyValue>Low+risk+0%e2%80%933+points%0aModerate+risk+4%e2%80%937+points%0aHigh+risk+%3e7+points</ScoreKeyValue>
                <InterpretationValue>%0a%0a</InterpretationValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </Screen_AssessTab>
      <ConsultationTab>
        <customTabName>Consultation</customTabName>
        <data>
          <EntryData>
            <Parent>Treatment_PlanningSection/ConsultationTab/</Parent>
            <Entry0>
              <PanelType>ConsultationEntryPanel</PanelType>
              <PanelData>
                <ReasonValue>Mr.+Wright+has+had+some+aberrant+drug-related+behavior+that+may+be+due+to+addiction+or+chronic+inadequately+relieved+pain.+Please+advise+on+the+possibility+of+a+taper+and+the+best+approach+if+you+agree+that%27s+a+good+choice.</ReasonValue>
                <OptionsValue>Consultation+Options%3a</OptionsValue>
                <data>
                  <EntryData>
                    <Parent>Treatment_PlanningSection/ConsultationTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>ConsultationOptionEntry</PanelType>
                      <PanelData>
                        <ConsultantValue>Pain+%26+Addiction+Specialist+</ConsultantValue>
                        <ResponseValue>First%2c+consider+that+his+behavior+could+be+the+result+of+poorly+managed+pain+rather+than+other+reasons+people+misuse+opioids.+I%27d+make+sure+he+has+maximal+support+pertaining+to+his+opioid+medications.+Support+should+include+a+highly+structured+treatment+with+frequent+follow-up+visits+monitoring+all+signs+of+addiction+including+urine+and+drug+tests%2c+prescription+drug+monitoring%2c+etc.+You%27ll+need+to+start+to+diversify+his+pain+management+first%2c+spreading+it+among+multiple+disciplines+and+interventions+before+you+reduce+to+opioids.+The+important+thing+is+to+keep+the+pain+managed+while+you+taper.+His+response+to+this+plan%2c+whether+he+can+tolerate+it%2c+will+help+us+understand+whether+there+is+a+need+for+addiction+treatment+as+well.+</ResponseValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Obtaining+a+consultation+is+appropriate+if+the+case+is+beyond+your+level+of+experience+or+training.+The+specialist+should+be+able+to+help+you+determine+whether+a+referral+is+indicated+or+whether+you+can+manage+the+case+with+some+guidance.+Unfortunately%2c+few+addiction+specialists+also+specialize+in+the+management+of+pain.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>ConsultationOptionEntry</PanelType>
                      <PanelData>
                        <ConsultantValue>Substance+use+counselor</ConsultantValue>
                        <ResponseValue>First%2c+consider+that+he+may+have+had+poorly+managed+pain+rather+than+the+other+reasons+people+misuse+opioids.+Then+I%27d+make+sure+he+has+maximal+support+pertaining+to+his+opioid+medications+you+know%2c+a+highly+structured+treatment+with+frequent+follow-up+visits+monitoring+all+signs+of+addiction+including+urine+and+drug+tests%2c+prescription+drug+monitoring%2c+etc.+You%27ll+need+to+start+to+diversify+his+pain+management+first%2c+spreading+it+among+multiple+disciplines+and+interventions+before+you+reduce+to+opioids.+The+important+thing+is+to+keep+the+pain+managed+while+you+taper.+His+response+to+this+plan%2c+whether+he+can+tolerate+it%2c+will+help+us+understand+whether+there+is+a+need+for+addiction+treatment+as+well.+</ResponseValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Obtaining+a+consultation+is+appropriate+if+the+case+is+beyond+your+level+of+experience+or+training.+The+specialist+should+be+able+to+help+you+determine+whether+a+referral+is+indicated+or+whether+you+can+manage+the+case+with+some+guidance.%0a%0aYou+are+more+likely+to+find+this+specialist+in+areas+that+don%e2%80%99t+have+major+medical+centers%2c+than+one+with+expertise+in+both+addiction+and+pain%2c+which+would+be+even+better.+</FeedbackValue>
                      </PanelData>
                    </Entry1>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </ConsultationTab>
      <TextboxTab>
        <customTabName>PreTreatment Dialogue</customTabName>
        <data>
          <EntryData>
            <Parent>Treatment_PlanningSection/PreTreatment DialogueTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <DialoguePin>
                  <dialogue>
                    <uid>Treatment_PlanningSection/PreTreatment DialogueTab/LabEntry: 0</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>Treatment_PlanningSection/PreTreatment DialogueTab/LabEntry: 0</Parent>
                        <Entry0>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>I+haven%27t+done+any+more+physical+therapy+because+it%27s+painful.+</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <QuestionValue>Choose+the+best+response+from+the+provider+regarding+physical+therapy%3a</QuestionValue>
                            <Image>
                            </Image>
                            <data>
                              <EntryData>
                                <Parent>Treatment_PlanningSection/PreTreatment DialogueTab/LabEntry: 0::LabEntry: 1</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Physical+therapy+is+designed+to+make+you+push+your+limits%2c+which+does+cause+some+discomfort.</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>It+is+important+to+be+open+and+honest+with+patients+about+the+challenges+of+recommended+treatment%2c+so+this+is+a+good+choice.+It+would+also+be+important+to+tell+him+about+the+benefits+of+physical+therapy+and+tips+to+make+it+less+painful.+</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>OK.+We%27ll+focus+on+other+treatments+since+physical+therapy+has+already+been+tried.</OptionValue>
                                    <OptionTypeValue>Incorrect</OptionTypeValue>
                                    <FeedbackValue>Just+because+physical+therapy+has+been+tried+before+doesn%27t+necessarily+mean+it+will+not+help.++</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry1>
                        <Entry2>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Physical+therapy+is+designed+to+make+you+push+your+limits+which+does+cause+some+discomfort.+Pushing+a+little+is+likely+to+make+you+stronger+and+function+better.+</dialogueText>
                          </PanelData>
                        </Entry2>
                        <Entry3>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>Yes%2c+I+suppose+you+are+right.+</dialogueText>
                          </PanelData>
                        </Entry3>
                        <Entry4>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Some+people+take+a+dose+of+pain+medication+about+an+hour+before+physical+therapy+to+make+it+more+comfortable+and+productive.+</dialogueText>
                          </PanelData>
                        </Entry4>
                        <Entry5>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I+hadn%e2%80%99t+thought+of+that.+That+makes+sense.+</dialogueText>
                          </PanelData>
                        </Entry5>
                        <Entry6>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I+want+to+work+with+you+to+get+you+the+best+pain+management+possible.+It+will+involve+medications+and+some+other+types+of+treatment.+It+is+important+for+you+to+use+all+of+the+recommended+treatments%2c+because+they+add+up.+They+work+together+to+provide+the+best+possible%2c+safe+pain+management.+</dialogueText>
                          </PanelData>
                        </Entry6>
                        <Entry7>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>That+sounds+good.+I+hope+it+makes+a+difference.+</dialogueText>
                          </PanelData>
                        </Entry7>
                        <Entry8>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <QuestionValue>Should+you+bring+up+taking+his+medications+according+to+the+prescribed+schedule+again+or+should+you+avoid+embarrassing+him+by+bringing+it+up+again+since+this+discussion+covers+it+well+enough%3f+</QuestionValue>
                            <Image>
                            </Image>
                            <data>
                              <EntryData>
                                <Parent>Treatment_PlanningSection/PreTreatment DialogueTab/LabEntry: 0::LabEntry: 8</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Avoid+making+the+patient+feel+bad+by+bringing+it+up+again+since+this+discussion+covers+it+well+enough.</OptionValue>
                                    <OptionTypeValue>Incorrect</OptionTypeValue>
                                    <FeedbackValue>It+is+important+to+obtain+an+explicit+understanding+with+Mr.+Wright+that+you+expect+him+to+follow+the+instructions+for+taking+his+medication+and+to+explain+why+this+is+important.+</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>Bring+up+taking+his+medications+according+to+the+prescribed+schedule.</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>Important+instructions+can+be+delivered+in+a+firm%2c+caring+tone%2c+without+shaming+the+patient.+It+is+important+to+obtain+an+explicit+understanding+with+Mr.+Wright+that+you+expect+him+to+follow+the+instructions+for+taking+his+medication+and+to+explain+why+this+is+important.+</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry8>
                        <Entry9>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>I+need+you+to+promise+you%27ll+stick+with+the+program+we+agree+upon%2c+including+taking+your+medications+according+to+the+schedule%2c+in+order+for+this+program+to+work+and+work+safely.</dialogueText>
                          </PanelData>
                        </Entry9>
                        <Entry10>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>That+makes+sense.+OK.+I%27ll+agree+to+that.</dialogueText>
                          </PanelData>
                        </Entry10>
                        <Entry11>
                          <PanelType>DialogueEntryTest2</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>Okay.+Let%27s+talk+about+the+non-medication+treatments+first.+</dialogueText>
                          </PanelData>
                        </Entry11>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <CustomContentValue>%3cb%3eInstructions%3c%2fb%3e%3a+Make+selections+for+what+the+provider+should+say+in+a+discussion+of+potential+treatments.+</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
    </_Treatment_PlanningSection>
    <_TreatmentSection>
      <sectionName>
      </sectionName>
      <TreatmentTab>
        <customTabName>Non-Pharm</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/Non-PharmTab/</Parent>
            <Entry0>
              <PanelType>TreatmentOtherEntryPanel</PanelType>
              <PanelData>
                <TreatmentValue>Physical+therapy</TreatmentValue>
                <ResponseValue>Motivating+Mr.+Wright+to+stay+with+his+physical+therapy+was+initially+challenging%2c+but+after+several+sessions%2c+Mr.+Wright+responded+well+to+the+palliative+and+strengthening+treatments.++</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Further+patient+education+regarding+physical+therapy+could+focus+on+its+benefits+of+pain+relief+and+strength+building+as+well+as+neuromuscular+education.+A+customized+exercise+plan+could+be+developed.++Even+if+PT+does+not+improve+Mr.+Wright%27s+pain%2c+an+important+benefit+of+physical+therapy+is+to+help+avoid+further+deterioration.+%0a%0aFurthermore%2c+a+multidisciplinary+pain+management+approach+is+important%2c+to+minimize+the+use+of+opioids+as+much+as+possible+given+his+high+level+of+risk.+</FeedbackValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>TreatmentOtherEntryPanel</PanelType>
              <PanelData>
                <TreatmentValue>Counseling</TreatmentValue>
                <ResponseValue>Mr.+Wright+gained+pain+coping+skills+slowly+and+was+able+to+stop+using+alcohol+to+avoid+drug+interactions.+His+depression+also+improved.+</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Because+Mr.+Wright+is+in+a+high-risk+group+for+opioid+use+disorder+or+misuse%2c+a+multidisciplinary+treatment+plan+for+pain+management+that+helps+minimize+that+risk+is+important%2c+including+behavioral+supports.+Some+of+his+dangerous%2c+aberrant+drug-related+behavior+seems+to+be+motivated+by+unmanaged+pain+so+developing+coping+skills+learning+how+to+avoid+misuse+is+important.+%0a%0aCognitive+behavioral+therapy+has+been+used+effectively+to+help+in+coping+with+pain.+Behavioral+therapy+could+also+be+used+to+help+him+avoid+cues+to+misuse+of+opioids.</FeedbackValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>TreatmentOtherEntryPanel</PanelType>
              <PanelData>
                <TreatmentValue>Weight+loss</TreatmentValue>
                <ResponseValue>Mr.+Wright%27s+osteoarthritis+progressed+less+rapidly+than+it+would+have+otherwise+and+he+noticed+a+little+better+functioning+and+less+pain+after+getting+his+BMI+down+to+25.+</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Because+Mr.+Wright+has+a+BMI+of+27.5%2c+which+is+overweight%2c+he+should+be+advised+to+lose+weight+until+he+is+at+a+weight+that+would+have+his+BMI+is+under+25.+The+American+Association+of+Osteopathic+Surgeons+guidelines+for+treatment+of+knee+osteoarthritis+(AAOS%2c+2011)+recommend+weight+loss+if+BMI+is+over+25.+</FeedbackValue>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>NonPharma</PanelType>
              <PanelData>
                <TreatmentValue>Self-managed+low+impact+exercise</TreatmentValue>
                <ResponseValue>Mr.+Wright+was+not+able+to+complete+these+exercises+at+first%2c+but+after+two+weeks+of+physical+therapy%2c+he+started+doing+them+under+the+supervision+of+the+physical+therapist.+After+a+month%2c+he+was+directed+to+continue+them+indefinitely+on+his+own.+Eventually%2c+after+two+months%2c+he+remarked+that+they+make+a+noticeable+improvement+in+his+pain+and+functioning.+</ResponseValue>
                <OptionTypeValue>Partially+Correct</OptionTypeValue>
                <FeedbackValue>Exercise+is+very+important+in+the+long+term+plan%2c+but+Mr.+Wright+may+not+be+able+to+engage+in+it+until+after+his+pain+is+a+little+better+managed.+The+American+Association+of+Osteopathic+Surgeons+guidelines+for+treatment+of+knee+osteoarthritis+(AAOS%2c+2011)+recommends+self-managed+low-impact+exercise.</FeedbackValue>
              </PanelData>
            </Entry3>
          </EntryData>
        </data>
      </TreatmentTab>
      <TextboxTab>
        <customTabName>Pain Med Info</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/Pain Med InfoTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3cb%3e%3ccolor%3d%23886382%3eBACKGROUND+INFORMATION+FOR+PHARMACOLOGICAL+TREATMENT%3c%2fcolor%3e%3c%2fb%3e%0a%0a%3cB%3ePrescribing+Guidelines+for+Chronic+Opioid+Therapy%3c%2fb%3e%0a%0d%0a+%e2%80%a2+Use+other+treatments+first+if+possible%3a+Non-opioid+pharmacologic+medication+and+nonpharmacologic+therapy+are+preferred+treatment+for+chronic+pain.+Only+consider+opioids+if+benefits+for+both+pain+and+functioning+are+likely+to+outweigh+risks.+If+opioids+are+prescribed%2c+minimize+their+use+by+combining+with+non-opioids+and+non-pharmacological+therapy.+%0d%0a%0d%0a+%e2%80%a2+Use+treatment+goals%3a+Set+realistic+treatment+goals+for+pain+and+function+at+the+outset.+Explain+that+treatment+will+continue+only+if+the+risk+vs.+benefit+ratio+is+favorable+with+%22clinically+meaningful+improvement.%22%0d%0a%0d%0a+%e2%80%a2+Discuss+known+risks+and+realistic+benefits+of+opioid+therapy+with+the+patient+before+starting.+Define+patient+and+clinician+responsibilities+for+managing+therapy.+%0d%0a%0d%0a+%e2%80%a2+Use+immediate-release%2c+not+extended-release%2flong-acting+opioids+(ER%2fLAs)+when+starting+opioid+therapy+for+chronic+pain.+Note+that+the+FDA%27s+REMS+(Risk+Evaluation+and+Mitigation+Strategy)++requires+that+the+pharmaceutical+companies+provide+training+for+opioid+prescribers+and+special+training+for+ER%2fLA+opioids.+%0a%0d+%0a+%e2%80%a2+Use+lowest+possible+dose%3a+Reassess+benefits+vs.+risks+carefully+when+considering+a+dosage+increase+to+%e2%89%a550+morphine+milligram+equivalents+(MME)%2fday.+Avoid+increasing+the+dose+to+%e2%89%a590+MME%2fday+or+carefully+justify.+%0d%0a%0d%0a+%e2%80%a2+Avoid+prescribing+opioids+together+with+benzodiazepines.+%0d%0a%0d%0a+%e2%80%a2+Treat+opioid+use+disorder%3a+Treat+or+arrange+treatment+for+opioid+use+disorder%2c+usually+with+medication-assisted+treatment%2c+i.e.%2c+buprenorphine+or+methadone%2c+in+combination+with+behavioral+therapy.+%0d%0a%0d%0a+%e2%80%a2+Evaluation+of+benefits+vs.+risks+is+ongoing%3a+Evaluate+benefits+and+risks+with+patients+within+1+to+4+weeks+of+starting+chronic+opioid+therapy+or+a+dose+increase.+Reevaluate+at+least+every+3+months.+Taper+to+a+lower+dosage+or+discontinue+opioids+if+benefits+do+not+exceed+risks.+%0d%0a%0d%0a%3cb%3eReference%3a%3c%2fb%3e+%0aRecommendations+from%3a+%3ci%3eCDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016%3c%2fi%3e+(Dowell%2c+et+al.%2c+2016)%0d.%0a%0a%3cb%3eUse+of+Treatment+Agreements+When+Prescribing+Chronic+Opioid+Therapy%3c%2fb%3e%0a%0aGuidelines+for+treating+chronic+pain+with+opioids+(CDC+-+Dowell+et+al.%2c+2016)+recommend+the+use+of+treatment+agreements+when+prescribing+chronic+opioids%3a%0d%0a%0d%0a+%e2%80%a2+Patient%2fprovider+treatment+agreements%3a+Consider+use+of+written+agreements+that+describe+responsibilities+of+both+the+patient+and+prescribing+provider+and+the+treatment+structure+that+helps+prevent+addiction%2c+misuse%2c+and+diversion.+Include+patient+education+on+using+as+directed%2c+safe+storage%2c+keeping+appointments%2c+not+sharing+medications%2c+etc.%0d%0a%0d%0a+%e2%80%a2+Increase+treatment+structure+for+higher+risk+patients%3a+The+treatment+agreement+should+describe+additional+requirements+for+patients+with+high+risk.+For+example%2c+more+frequent+appointments+and+urine+drug+testing+might+be+required+with+higher+risk.%0d%0a%0d%0a+%e2%80%a2+Plan+for+stopping+opioid+treatment+before+starting%3a+Describe+a+plan+in+the+treatment+agreement+that+includes+the+conditions+under+which+treatment+will+be+stopped%2c+and+a+plan+for+tapering+and+providing+psychosocial+supports+when+stopping.%0d%0a%0d%0a%0d%0a%3cb%3eTherapeutic+Injection+for+Knee+Pain%3c%2fb%3e%0a%0aHyaluronic+acid+or+hyaluronan+injections+are+used+to+introduce+a+lubricating+fluid+into+the+joint.+This+treatment+is+often+used+in+knee+osteoarthritis.+%0d%0a%0a%0a%3cb%3eDefinitions%3a+%0d%3c%2fb%3e%0a%0aA+%22rescue+medication%22+in+pain+management+is+an+analgesic+medication+for+when+the+patient+still+has+pain+after+taking+regular+pain+medications.+%0d%0a%0d%0a%22Adjuvant%22+pain+medications+are+added+to+analgesics+to+improve+the+amount+of+pain+management.+Antidepressants+and+anti-epileptics+are+most+commonly+used.</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <TreatmentTab>
        <customTabName>Rx</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/RxTab/</Parent>
            <Entry0>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>Prescribe+current+opioid+but+start+a+slow+taper</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>Mr.+Wright+was+able+to+tolerate+a+very+slow+taper+of+his+opioid+dose.+The+tapering+process+included+adding+non-opioid+analgesics+and+adjuvants+as+well+as+non-pharmacological+treatments+in+a+comprehensive+multidisciplinary+treatment+plan.+With+this+approach%2c+he+was+better+able+to+stick+to+his+dosing+schedule+and+eventually+felt+more+in+control+and+less+depressed</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Currently%2c+Mr.+Wright+has+insufficient+pain+management%2c+but+this+should+be+addressed+via+the+addition+of+non-opioid+analgesics+and+adjuvant+medications+plus+the+addition+of+non-pharmacological+therapies+in+a+comprehensive+multidisciplinary+pain+management+plan.+Tapering+can+sometimes+be+accomplished+slowly+without+any+decrease+in+pain+relief.+%0a%0aNote+that+the+FDA+requires+manufacturers+of+extended-release+opioids+to+train+providers+in+their+safe+and+appropriate+use.+%0d</FeedbackValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>Medication-assisted+treatment+for+opioid+use+disorder</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>Mr.+Wright+may+not+need+treatment+for+opioid+use+disorder.+</ResponseValue>
                <OptionTypeValue>Partially+Correct</OptionTypeValue>
                <FeedbackValue>Not+the+best+choice+at+this+time%2c+but+may+be+needed+if+other+treatments+fail.+If+that+were+to+happen%2c+medication-assisted+treatment+for+opioid+use+disorder+(e.g.%2c+buprenorphine+or+methadone)+could+be+considered+along+with+providing+non-opioid+pain+relief.+Patients+often+experience+some+pain+relief+from+buprenorphine+itself+while+it+is+being+used+to+treat+opioid+use+disorder.</FeedbackValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>Add+immediate-release+oxycodone+for+breakthrough+pain</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>If+Mr.+Wright+is+prescribed+the+immediate+release+oxycodone+he+would+be+more+likely+to+overdose+or+develop+opioid+use+disorder.+</ResponseValue>
                <OptionTypeValue>Incorrect</OptionTypeValue>
                <FeedbackValue>When+a+patient+experiences+breakthrough+pain+despite+taking+opioids%2c+it+does+not+necessarily+mean+that+more+opioids+are+needed.+An+NSAID+or+other+non-opioid+analgesic+should+be+added+instead.+A+multidisciplinary+approach+to+pain+management%2c+including+non-pharmacological+treatments%2c+is+also+needed.+%0a%0aWith+the+unsafe+behavior+of+taking+his+medication+early%2c+prescribing+immediate+release+oxycodone+to+take+as+needed+would+probably+be+too+dangerous%2c+with+risk+of+overdose+and+addiction.+%0d</FeedbackValue>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>Naloxone+overdose+kit</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>Mr.+Wright+never+needed+to+use+the+naloxone+overdose+kit+but+was+glad+to+have+it.+He+told+people+who+are+close+to+him+about+the+kit+and+how+to+use+it+if+he+should+be+found+non-responsive+from+an+apparent+overdose.+</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Current+guidelines+for+managing+chronic+pain+recommend+offering+to+prescribe+a+naloxone+kit+to+treat+overdose+when+chronic+opioids+are+prescribed+(Dowell+et+al.%2c+2016).+</FeedbackValue>
              </PanelData>
            </Entry3>
            <Entry4>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>Increase+dose+of+long-acting+opioid</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>Mr.+Wright%27s+pain+was+managed+well+by+the+increased+dose+initially%2c+but+soon+he+was+reporting+inadequate+pain+relief+and+breakthrough+pain+again.+Furthermore%2c+his+more+addictive+behavior+increased.+</ResponseValue>
                <OptionTypeValue>Incorrect</OptionTypeValue>
                <FeedbackValue>Mr.+Wright+has+moderate+to+high+risk+for+taking+opioids+and+opioids+are+not+the+first-line+recommended+treatment+for+his+condition.+A+taper+off+opioids+if+possible%2c+plus+a+multidisciplinary+approach+to+pain+management%2c+including+non-opioid+analgesics+and+non-pharmacological+treatments%2c+is+needed+instead.+%0a%0aNote+that+the+FDA+requires+manufacturers+of+extended-release+opioids+to+train+providers+in+their+safe+and+appropriate+use.+%0d</FeedbackValue>
              </PanelData>
            </Entry4>
            <Entry5>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>NSAID+analgesic</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>Mr.+Wright+was+surprised+to+find+that+an+over-the-counter%2c+easily+absorbed+ibuprofen%2c+taken+only+when+his+pain+flared+up%2c+provided+significant+pain+relief.</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Non-opioid+analgesics+should+be+used+to+spare+the+dose+of+opioids+that+is+needed+for+pain+management+(Dowell+et+al.%2c+2016).++Mr.+Wright+had+stopped+using+NSAIDs+and+acetaminophen+because+they+did+not+provide+sufficient+pain+relief+by+themselves.+He+needs+to+be+taught+that+NSAIDs+can+be+a+part+of+a+comprehensive+treatment+plan+and+that+all+components+of+that+plan+act+together+to+manage+his+pain+safely.+</FeedbackValue>
              </PanelData>
            </Entry5>
            <Entry6>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>Docusate+sodium+stool+softener</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>Mr.+Wright+continued+to+have+no+problems+from+his+opioid-induced+constipation.</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Docusate+stool+softener+is+an+over-the-counter+medication+that+is+one+of+the+first-choice+medications+when+treating+opioid-induced+constipation+(Kumar+et+al.%2c+2014).+It+should+be+prescribed+as+long+as+Mr.+Wright+continues+taking+opioids.+He%27s+had+constipation+before+from+taking+opioids%2c+so+it+is+likely+to+continue.+The+constipation+was+managed+previously+with+this+stool+softener%2c+so+this+is+a+good+place+to+start+with+his+treatment.+It+avoids+potential+drug+interactions+with+certain+laxatives.+Bulk+laxatives+containing+psyllium+are+not+recommended+due+to+risk+of+bowel+impaction.+</FeedbackValue>
              </PanelData>
            </Entry6>
            <Entry7>
              <PanelType>TreatmentPharmaEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>Continue+antidepressant</MedicationValue>
                <DoseValue>
                </DoseValue>
                <HowToTakeValue>
                </HowToTakeValue>
                <ResponseValue>Mr.+Wright+said+he+thought+that+his+pain+was+better+managed+with+his+antidepressant+and+he+continued+to+have+no+significant+side+effects.+His+depression+continued+to+be+minimal.+</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Managing+depression+is+an+important+part+of+chronic+pain+management.+Antidepressants+also+sometimes+help+with+chronic+pain+as+adjuvant+treatment%2c+especially+neurological+pain.+</FeedbackValue>
              </PanelData>
            </Entry7>
          </EntryData>
        </data>
      </TreatmentTab>
      <TreatmentTab>
        <customTabName>Surg</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/SurgTab/</Parent>
            <Entry0>
              <PanelType>TreatmentOtherEntryPanel</PanelType>
              <PanelData>
                <TreatmentValue>Surgical+consultation</TreatmentValue>
                <ResponseValue>At+this+consultation%2c+Chad+learned+that+arthroplasty+is+likely+to+help+in+his+case.+He+is+not+interested+at+this+time+due+to+cost+and+time+factors.+He+asks+the+surgeon+about+alternatives+to+more+surgery+and+learns+that+a+hyaluronic+acid+injection+acts+as+a+joint+lubricant+and+may+help+his+knee%2c+but+the+effectiveness+is+uncertain+(AAOS%2c+2011).+He+makes+an+appointment+to+get+hyaluronic+acid+injections+because+they+have+helped+in+the+past.++</ResponseValue>
                <OptionTypeValue>Correct</OptionTypeValue>
                <FeedbackValue>Referral+to+a+surgeon+is+indicated+because+Mr.+Wright+has+not+had+a+surgical+evaluation+recently.+There+may+be+newer+techniques+or+materials+to+increase+the+range+of+options+since+the+last+time+he+saw+a+surgeon.++For+example%2c+he+might+benefit+from+therapeutic+injections+(e.g.%2c+hyaluronic+acid)+or+joint+replacement+although+the+evidence+is+mixed+for+their+effectiveness+in+osteoarthritis+(AAOS%2c+2011).+He+also+might+benefit+from+arthroplasty+(knee+replacement+surgery).+</FeedbackValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>TreatmentOtherEntryPanel</PanelType>
              <PanelData>
                <TreatmentValue>No+surgical+consultation+needed</TreatmentValue>
                <ResponseValue>No+results</ResponseValue>
                <OptionTypeValue>Incorrect</OptionTypeValue>
                <FeedbackValue>Referral+to+a+surgeon+is+indicated+because+Mr.+Wright+hasn%27t+had+a+surgical+evaluation+recently.+There+may+be+newer+techniques+or+materials+to+increase+the+range+of+options+since+the+last+time+he+saw+a+surgeon+that+he+should+be+aware+of+for+planning+purposes.++For+example%2c+he+might+benefit+from+therapeutic+injections+(e.g.%2c+hyaluronic+acid)+or+knee+replacement+surgery+(arthroplasty).</FeedbackValue>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </TreatmentTab>
      <QuizTab>
        <customTabName>Tx Quiz</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/Tx QuizTab/</Parent>
            <Entry0>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>When+evaluating+patients+for+potential+chronic+opioid+therapy%2c+physicians+should+triage+patients+to+stratify+risk.+%0a%0aChoose+which+of+the+following+describes+a+patient+having+moderate-risk+for+opioid+overdose%2c+diversion%2c+and+addiction%3a++(Choose+all+that+apply)</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Checkboxes</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>TreatmentSection/Tx QuizTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Current+substance+use+problems</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Having+current+substance+use+problems+would+put+the+patient+in+a+severe+risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+does+not+have+a+substance+use+problem+other+than+his+misuse+of+opioids+that+were+prescribed+for+him.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Comorbid+minor+or+past+major+mental+health+problem</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>A+comorbid+minor+or+past+major+mental+health+problem+would+put+the+patient+in+a+severe-risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+has+depression+that+is+fairly+well-managed+with+an+anti-depressant.+If+this+were+his+only+problem%2c+he+would+be+considered+to+have+moderate+risk.+</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Family+history+of+substance+abuse</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Family+history+of+substance+use+would+put+the+patient+in+a+severe-risk+category%2c+not+moderate.%0a%0aMr.+Wright%27s+family+did+not+have+a+history+of+substance+abuse.+</FeedbackValue>
                      </PanelData>
                    </Entry2>
                    <Entry3>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Major+untreated+mental+health+problems</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Major+untreated+mental+health+problems+would+put+the+patient+in+a+severe-risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+has+depression+that+is+fairly+well-managed+with+an+anti-depressant%2c+which+is+not+a+major+mental+health+problem.+</FeedbackValue>
                      </PanelData>
                    </Entry3>
                    <Entry4>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Changing+opioid+medication+dosage+without+consulting+providor</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Changing+opioid+medication+dosage+without+consulting+providor+puts+the+patient+in+a+severe-risk+category+rather+than+moderate+risk.+%0a%0aMr.+Wright+changed+the+dose+of+his+opioid+medication+without+consulting+a+provider+an+so+should+be+considered+high-risk.</FeedbackValue>
                      </PanelData>
                    </Entry4>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>To+minimize+risk+for+addiction%2c+the+best+strategy%2c+if+opioids+are+clearly+needed%2c+for+starting+a+patient+on+chronic+opioid+therapy+is%3a++(Choose+the+best+answer)%0d</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>TreatmentSection/Tx QuizTab/LabEntry: 1</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Use+long+acting%2fextended+release+opioids+on+a+schedule+for+pain</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Use+long+acting%2fextended+release+opioids+as+needed+for+pain</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Use+immediate+release+opioids+as+needed+for+pain</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+or+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</FeedbackValue>
                      </PanelData>
                    </Entry2>
                    <Entry3>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Use+immediate+release+opioids+on+a+schedule+for+pain</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Using+immediate-release+opioids+on+a+schedule+is+the+best+option+to+avoid+addiction.+The+CDC%27s+guidelines+for+chronic+opioid+therapy+(Dowell+et+al.%2c+2016)+recommend+that+patients+not+be+placed+on+long-acting+or+extended-release+opioids+initially.+Describing+a+schedule+for+taking+opioids+is+better+than+as-needed+so+as+to+reduce+the+potential+for+reinforcement+which+could+contribute+to+developing+addiction.+</FeedbackValue>
                      </PanelData>
                    </Entry3>
                  </EntryData>
                </data>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </QuizTab>
      <QuizTab>
        <customTabName>Pt Ed</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/Pt EdTab/</Parent>
            <Entry0>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>Which+of+the+following+patient+education+topics+is+important+currently%3f</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Checkboxes</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>TreatmentSection/Pt EdTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Patient+information+flier+regarding+taking+opioids+safely</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Mr.+Wright+should+be+given+instructions+for+safe+use+of+opioids+while+he+is+still+taking+them.+This+should+include%3a+%0a+%e2%80%a2+What+not+to+take+with+opioids+(benzodiazepines%2c+alcohol%2c+and+sedative-hypnotic+substances)%0a+%e2%80%a2+What+to+do+in+the+event+of+an+overdose+(contact+EMS+and+administer+naloxone)%0a+%e2%80%a2+Safe+storage+and+disposal%0a+%e2%80%a2+Other+safeguards+against+diversion.+%0a%0aMany+practices+also+send+a+follow-up+message%2c+such+as+%22Notes+from+Your+Recent+Visit+to+(Name+of+Practice)%22+that+repeat+any+instructions+that+were+given.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Information+on+opioids+and+constipation</OptionValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>This+is+important+information+when+starting+chronic+opioid+therapy+or+if+constipation+develops+as+a+symptom+later.+However%2c+Mr.+Wright+has+been+treated+successfully+for+his+opioid-induced+constipation+using+docusate+stool+softener+for+many+years+and+so+probably+does+not+need+this+information.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>Which+of+the+following+is+best+to+help+Mr.+Wright+adhere+to+his+treatment+if+opioids+are+prescribed%3f+(Choose+the+best+answer)</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>TreatmentSection/Pt EdTab/LabEntry: 1</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Written+and+signed+provider-patient+treatment+agreement</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Written+provider-patient+treatment+agreements+are+recommended+by+the+CDC%27s+guidelines+for+chronic+pain+management+with+opioids+(Dowell+et+al.%2c+2016).+These+agreements+spell+out+your+office+policies+regarding+chronic+opioid+therapy+and+any+additional+rules+or+regulations+needed+for+a+specific+patient.+Treatment+agreements+typically+describe+policies+for%3a%0a+%e2%80%a2+missed+appointments%0a+%e2%80%a2+lost+medications%0a+%e2%80%a2+early+refill+requests%0a+%e2%80%a2+urine+drug+tests%0a+%e2%80%a2+when+treatment+would+be+discontinued%0a+%e2%80%a2+a+plan+for+ending+treatment+%0a%0aA+patient+with+high+risk+might+additionally+be+required+to+bring+in+their+prescription+bottle+for+you+to+reconcile+the+number+of+pills+in+it+with+the+number+expected.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Treatment+contract</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Contracts+are+no+longer+recommended+for+these+agreements.+The+consensus+in+the+medical+profession+is+that+forming+a+contract+is+not+appropriate.+It+does+not+fit+within+the+patient-centered+care+model+in+which+the+patient+is+empowered+to+take+an+active+partner-like+role+in+his+or+her+treatment.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Making+eye+contact+and+asking+for+agreement+with+a+handshake+after+discussing+policies</OptionValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>This+would+be+a+good+approach+for+some+patients+but+most+patients+on+chronic+opioid+therapy+need+more+structured+guidelines+and+policies.+The+written+treatment+agreement+is+a+good+compromise+between+a+contract+and+a+handshake.</FeedbackValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </QuizTab>
      <ReferralTab>
        <customTabName>Referral</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/ReferralTab/</Parent>
            <Entry0>
              <PanelType>ReferralEntryPanel</PanelType>
              <PanelData>
                <ReasonValue>Mr.+Wright+increased+his+opioid+dose+by+increasing+frequency+without+medical+supervision.+Evaluate+for+Opioid+Use+Disorder</ReasonValue>
                <data>
                  <EntryData>
                    <Parent>TreatmentSection/ReferralTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>ReferralOptionEntry</PanelType>
                      <PanelData>
                        <OptionValue>A+physician+specializing+in+addiction+and+pain</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Recommending+evaluation+for+substance+use+disorder+is+indicated%2c+especially+if+Mr.+Wright+continues+to+modify+the+way+he+takes+his+medication+on+his+own.+An+addiction+specialist+who+specializes+in+pain+would+also+be+ideal.+Unfortunately%2c+there+aren%27t+very+many+individuals+having+these+qualifications.</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>ReferralOptionEntry</PanelType>
                      <PanelData>
                        <OptionValue>Substance+abuse+counselor</OptionValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>Recommending+evaluation+for+substance+use+disorder+is+indicated%2c+especially+if+Mr.+Wright+continues+to+modify+the+way+he+takes+his+medication+on+his+own.++A+substance+abuse+counselor+could+evaluate+Mr.+Wright+for+opioid+use+disorder.+However%2c+they+typically+do+not+have+much+training+in+pain+management.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>ReferralOptionEntry</PanelType>
                      <PanelData>
                        <OptionValue>No+referral+needed.+Opioid+risk+has+been+ruled+out.+</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Recommending+evaluation+for+substance+use+disorder+is+indicated%2c+especially+if+Mr.+Wright+continues+to+modify+the+way+he+takes+his+medication+on+his+own.+</FeedbackValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </ReferralTab>
      <QuizTab>
        <customTabName>Follow Up</customTabName>
        <data>
          <EntryData>
            <Parent>TreatmentSection/Follow UpTab/</Parent>
            <Entry0>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>Choose+the+best+follow-up+plan+from+the+following%3a+(Choose+best+answer)</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>TreatmentSection/Follow UpTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Follow+up+at+1+week</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Even+without+risk%2c+initial+follow-up+of+around+1-2+weeks+or+sooner+when+there+is+high+risk+indicated+in+chronic+opioid+therapy+to+check+on+pain+control+and+side+effects.+Frequent+follow-up+continues+to+be+important+in+chronic+opioid+therapy+when+the+patient+has+relatively+higher+risk%2c+as+in+this+case+where+he+has+a+history+of+modifying+his+dose+without+medical+approval+and+combining+alcohol+use+with+opioid+use.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Follow-up+optional</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Even+without+risk%2c+initial+follow-up+of+around+1-2+weeks+or+sooner+when+there+is+high+risk+is+indicated+in+chronic+opioid+therapy+to+check+on+pain+control+and+side+effects.+Frequent+follow-up+continues+to+be+important+in+chronic+opioid+therapy+when+the+patient+has+relatively+higher+risk%2c+as+in+this+case+he+has+a+history+of+modifying+his+dose+without+medical+approval+and+combining+alcohol+use+with+opioid+use.+</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Follow-up+not+needed</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Even+without+risk%2c+initial+follow-up+of+around+1-2+weeks+or+sooner+when+there+is+high+risk+is+indicated+in+chronic+opioid+therapy+to+check+on+pain+control+and+side+effects.+Frequent+follow-up+continues+to+be+important+in+chronic+opioid+therapy+when+the+patient+has+relatively+higher+risk%2c+as+in+this+case+he+has+a+history+of+modifying+his+dose+without+medical+approval+and+combining+alcohol+use+with+opioid+use.+</FeedbackValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>How+often+should+the+Prescription+Drug+Monitoring+Plan+(PDMP)+be+examined+as+Mr.+Wright%27s+treatment+continues%3f+(Choose+best+answer)</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>TreatmentSection/Follow UpTab/LabEntry: 1</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Annually</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped+as+well.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Before+initial+prescription+for+opioids</OptionValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+also+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Periodically+before+and+during+chronic+opioid+therapy</OptionValue>
                        <OptionTypeValue>Partially+Correct</OptionTypeValue>
                        <FeedbackValue>This+is+only+partially+correct%2c+because+PDMP+reports+should+continue+after+opioids+are+tapered.+The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped%2c+as+well.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</FeedbackValue>
                      </PanelData>
                    </Entry2>
                    <Entry3>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Before+each+prescription+of+opioids+and+periodically+after+tapering+off+opioids</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>The+CDC+guidelines+for+opioid+prescribing+recommend+checking+the+PDMP+regularly%2c+considering+a+check+before+each+prescription+in+chronic+opioid+therapy+(Dowell+et+al.%2c+2016).+For+patients+who+are+being+tapered+off+opioids%2c+it+is+a+good+idea+to+continue+to+check+the+PDMP+periodically+after+opioids+are+stopped%2c+as+well.+This+will+help+assure+that+patients+are+not+seeking+opioids+from+another+provider.+</FeedbackValue>
                      </PanelData>
                    </Entry3>
                  </EntryData>
                </data>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </QuizTab>
    </_TreatmentSection>
    <_SummarySection>
      <sectionName>
      </sectionName>
      <TextboxTab>
        <customTabName>Summary</customTabName>
        <data>
          <EntryData>
            <Parent>SummarySection/SummaryTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>%3cb%3eCASE+SUMMARY%3c%2fB%3e%0aChad+Wright%2c+34+yo+male.+%0a%0a%3cb%3eHISTORY%3c%2fb%3e%0a%3cb%3eCC%3a%3c%2fb%3e+Requests+opioid+prescription+for+chronic+knee+pain.+%0a%3cb%3eHPI%3a%3c%2fb%3e+Football+injury+to+right+knee+in+2002+with+subsequent+pain.+Treated+surgically+twice+(2002%2c+2008).+Treated+in+recent+years+with+opioids%2flong-acting+oxycodone+for+several+months.+%0a+%e2%80%a2+Pain+is+usually+mild+at+rest+and+moderately+severe+with+extensive+activity+but+fairly+well-controlled+with+opioids.+%0a+%e2%80%a2+History+of+depression+that+he+relates+to+living+with+chronic+pain+and+its+limitations.+Treated+with+antidepressant+Cymbalta%2c+although+he+still+has+mild+symptoms.+%0a+%e2%80%a2+History+of+aberrant+drug-related+behavior+of+increasing+opioid+doses+himself+without+discussing+with+his+prescriber%2c+who+discharged+him+from+the+practice+after+several+early+prescription+renewals.%0a+%e2%80%a2+He+requests+a+prescription+for+long-acting+oxycodone+with+additional+tablets+for+when+his+pain+is+worse.+%0a+%e2%80%a2+Drug+interactions%3a+mild+alcohol+use+with+duloxetine+and+oxycodone.+%0a+%e2%80%a2+Takes+stool+softener+for+constipation.%0a%0a%3cb%3eEVALUATION%3c%2fb%3e%0a%3cb%3eSubstance+Use%3c%2fb%3e%3a%0a+%e2%80%a2+Positive+Substance+Use+Screening+CAGE-AID%3a+Score+3%2b+-+Clinically+significant.+Further+evaluation+for+opioid+use+disorder+is+indicated.+%0a+%e2%80%a2+Alcohol+screening+(AUDIT)%3a+Score+4+-+Below+clinical+significance.+Preventive+education+and+re-screen+in+6+months.%0a+%e2%80%a2+Positive+Opioid+Risk+Tool+(ORT)%3a+Moderate+to+High+risk.+Recommendation+-+Avoid+opioids+and+use+alternatives+such+as+NSAIDs.+If+opioids+are+needed%2c+use+a+high+level+of+treatment+structures+after+warmup.+%0a%3cb%3eUrine+Toxicology%3a+%3c%2fb%3e%0aPositive+only+for+oxycodone+and+oxymorphone%2c+as+expected+due+to+his+recent+use+of+oxycodone.%0a%3cb%3eRadiology%3c%2fb%3e%0aMRI+of+right+knee+shows+patellofemoral+and+tibiofemoral+osteoarthritis%0a%0a%3cb%3eDIAGNOSIS%3c%2fb%3e%0a+%e2%80%a2+Chronic+osteoarthritis%2c+right+knee%0a+%e2%80%a2+Opioid-induced+constipation%0a+%e2%80%a2+Major+Depression+treated+with+antidepressants+but+still+has+mild+depression.+Potential+drug+interaction%3a+duloxetine+and+alcohol+use%0a%0a%3cb%3eTREATMENT+PLAN%3c%2fb%3e%0a+%e2%80%a2+Orthopedic+Surgery+referral+to+evaluate+treatment+options%0aSurgery+Referral+Results%3a+Mr.+Wright+will+try+hyaluronic+acid+injections%2c+but+is+postponing+arthroplasty%2c+which+the+surgeon+recommended.%0a+%e2%80%a2+Counseling+referral+for+residual+depression+and+pain+coping+skills+as+an+alternative+to+drinking+alcohol.%0a+%e2%80%a2+Physical+therapy%0a+%e2%80%a2+Follow-up+plan%3a+Office+visits+every+1-2+weeks+initially%2c+re-evaluate+risk+after+one+month.++</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <TextboxTab>
        <customTabName>Learning Points</customTabName>
        <data>
          <EntryData>
            <Parent>SummarySection/Learning PointsTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>1.+The+diagnosis+of+opioid+use+disorder+involves+a+pattern+of+opioid+use+causing+clinically+significant+impairment+or+distress%2c+and+at+least+two+diagnostic+criteria.+Source+DSM-5+(APA%2c+2013+in+PCSS%2c+2013).%0a%0a2.+Non-opioid+medications+and+nonpharmacological+therapy+are+the+preferred+first-line+treatment+for+chronic+pain+according+to+the+CDC%27s+evidence-based+guidelines+(Dowell+et+al.%2c+2016).+%0a%0a3.+Only+consider+opioids+if%3a%0a++++%e2%80%a2+First-line+treatments+are+not+effective.%0a++++%e2%80%a2+Pain+is+moderate+to+severe.%0a++++%e2%80%a2+Benefits+for+both+pain+and+functioning+are+likely+to+outweigh+risks.%0a%0a4.+If+opioids+are+prescribed%3a%0a++++%e2%80%a2+Prescribing+a+3-day+supply+is+sufficient+for+most+acute+pain%2c+typically+no+more+than+7+days+is+needed.+%0a++++%e2%80%a2+Minimize+dose+by+combining+with+other+therapies+and+multidisciplinary+treatment.%0a++++%e2%80%a2+Use+written+provider-patient+treatment+agreements.%0a++++%e2%80%a2+Monitor+patients+regularly%2c+including+an+assessment+for+continued+need.%0a%0a5.+Prior+to+prescribing+chronic+opioids%3a%0a++++%e2%80%a2+Obtain+a+complete+pain+history%2c+including+all+past+treatments+that+have+been+tried+and+response+to+treatment.%0a++++%e2%80%a2+Consult+your+state%27s+Prescription+Drug+Monitoring+Program+(PDMP).</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <TextboxTab>
        <customTabName>References</customTabName>
        <data>
          <EntryData>
            <Parent>SummarySection/ReferencesTab/</Parent>
            <Entry0>
              <PanelType>TextboxPanel</PanelType>
              <PanelData>
                <CustomContentLabelValue>Custom+Content%3a</CustomContentLabelValue>
                <CustomContentValue>American+Academy+of+Orthopedic+Surgeons.+Treatment+of+Osteoarthritis+of+the+Knee+2nd+edition.+2011%3b+Available+at%3a+https%3a%2f%2fwww.aaos.org%2fresearch%2fguidelines%2fOAKSummaryofRecommendations.pdf%0a%0aAmerican+Psychiatric+Association.+Diagnostic+and+Statistical+Manual+of+Mental+Disorders%2c+Fifth+Edition.+Washington%2c+DC%2c+American+Psychiatric+Association.+2013+page+541.%0a%0aPCSS-MAT%2fAPA.+Opioid+use+disorder+diagnostic+criteria.+Reprinted+from+Diagnostic+and+Statistical+Manual+of+Mental+Disorders%2c+Fifth+Edition%2c+(Copyright+2013)+American+Psychiatric+Association.+Available+at%3a+http%3a%2f%2fpcssmat.org%2fwp-content%2fuploads%2f2014%2f02%2f5B-DSM-5-Opioid-Use-Disorder-Diagnostic-Criteria.pdf+Accessed+on%3a+2017-04-10.%0a%0d%0aBabor+TF%2c+de+la+Fluente+JF%2c+Saunders+J%2c++Grant+M.+AUDIT%3a+The+Alcohol+Use+Disorders+Identification+Test%3a+guidelines+for+use+in+primary+health+care.+Generva%2c+Switzerland%3a+World+Health+Organization.+1992.+%0a%0aBeck+AT%2c+Steer+RA%2c+Carbin+MG.+Psychometric+properties+of+the+Beck+Depression+Inventory%3a+Twenty-five+years+of+evaluation.+Clinical+Psychology+Review.+1988%3b+8(1)%3a77-100.%0a%0aBelgrade+M%2c+Schamber+CD%2c+Lindgren+BR.+The+DIRE+score.+Predicting+outcomes+of+opioid+prescribing+for+chronic+pain.+The+Journal+of+Pain.+2006%3b+7(9)%3a+671-81.+Available+at%3a+http%3a%2f%2fwww.jpain.org%2farticle%2fS1526-5900(06)00626-2%2fabstract.+Accessed+on%3a+2017-04-10.+%0a%0aBriley+M%2c+Moret+C.+Treatment+of+comorbid+pain+with+serotonin+norepinephrine+reuptake+inhibitors.+CNS+Spectr.+2008%3b+13(7)%3a+22-26.%0d%0a%0aDowell+D%2c+Haegerich+TM%2c+Chou+R.+CDC+Guideline+for+Prescribing+Opioids+for+Chronic+Pain+%e2%80%94+United+States%2c+2016.+MMWR+Recomm+Rep.+2016%3b+ePub%3a+March+2016%3a+DOI%3a+http%3a%2f%2fdx.doi.org%2f10.15585%2fmmwr.rr6501e1er.+Available+at%3a+https%3a%2f%2fwww.cdc.gov%2fmmwr%2fvolumes%2f65%2frr%2frr6501e1.htm.+Accessed+on%3a+2017-04-10.%0a%0aFDA.+Drug+Safety+and+Availability+-+FDA+Drug+Safety+Communication%3a+FDA+recommends+against+the+continued+use+of+propoxyphene.+https%3a%2f%2fwww.fda.gov%2fDrugs%2fDrugSafety%2fucm234338.htm.+Accessed+April+7%2c+2017.+%0a%0aKellgren+JH%2c+Lawrence+JS.+Radiological+assessment+of+osteo-arthrosis.+Ann.+Rheum.+Dis.+2000%3b16+(4)%3a+494-502.+Available+at+https%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpubmed%2f13498604.+Accessed+5%2f29%2f2017.%0a%0aKrebs+EE%2c+Lorenz+KA%2c+Bair+MJ.+Development+and+initial+validation+of+the+PEG%2c+a+three-item+scale+assessing+pain+intensity+and+interference.+J+Gen+Intern+Med.+2009%3b+24(6)%3a+733-738.+Available+at%3a+http%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpmc%2farticles%2fPMC2686775%2f+Accessed+on%3a+2017-04-10.%0a%0aKumar+L%2c+Barker+C%2c+Emmanuel+A.+Opioid-induced+constipation%3a+Pathophysiology%2c+clinicial+consequences%2c+and+management.+Gastroenterology+Research+and+Practice.+2014%3b+2014.+Available+at%3a+https%3a%2f%2fwww.hindawi.com%2fjournals%2fgrp%2f2014%2f141737%2f.%0a%0aMarshall%2c+PS.+Physical+Functional+Ability+Questionnaire+(FAQ5).+In%3a+Assessment+and+Management+of+Chronic+Pain%2c+5th+edition.+Institute+for+Clinical+Systems+Improvement+.+2011%3b+14%3a+Appendix+C%3a99.+Available+at%3a+http%3a%2f%2fwww.generationsprimarycare.com%2fassets%2fpain-contract.pdf+Accessed+on%3a+2017-04-10.%0a%0aMerrill+JO%2c+Von+Korff+M%2c+Banta-Green+CJ%2c+et+al.+Prescribed+opioid+difficulties%2c+depression+and+opioid+dose+among+chronic+opioid+therapy+patients.+General+Hospital+Psychiatry.+2012%3b+34%3a+581-587.+Available+at%3a+http%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpubmed%2f22959422+Accessed+on%3a+2017-04-17.%0a%0aPani+PP%2c+Vacca+R%2c+Troqu+E%2c+Amato+L%2c+Davoli+M.+Pharmacological+treatment+for+depression+during+opioid+agonist+treatment+for+opioid+dependence.+Cochrane+Database+of+Systematic+Reviews.+2010%3b+8(9)%3a+CD008373.+Available+at%3a+http%3a%2f%2fonlinelibrary.wiley.com%2fdoi%2f10.1002%2f14651858.CD008373.pub2%2fabstract+Accessed+on%3a+2017-04-10.%0a%0aPassik+SD%2c+Kirsh+KL%2c+Casper+D.+Addiction-related+assessment+tools+and+pain+management%3a+instruments+for+screening%2c+treatment+planning+and+monitoring+compliance.+Pain+Med.+2008%3b+9%3a+S145-S166.%0d%0a%0d%0aWebster+LR.+Predicting+aberrant+behaviors+in+opioid-treated+patients%3a+Preliminary+validation+of+the+opioid+risk+tool.+Pain+Medicine.+2005%3b6(6)%3a432-442.+Available+at%3a+https%3a%2f%2fwww.ncbi.nlm.nih.gov%2fpubmed%2f16336480.+Accessed+on%3a+2017-04-10.</CustomContentValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </TextboxTab>
      <QuizTab>
        <customTabName>Post-Test</customTabName>
        <data>
          <EntryData>
            <Parent>SummarySection/Post-TestTab/</Parent>
            <Entry0>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>%3cb%3eCase+Scenario%3c%2fb%3e%0aEllen+Small%2c+age+37%2c+has+been+taking+hydrocodone+without+a+prescription.+She+started+by+taking+opioids+she+had+left+over+from+dysmenorrhea+that+resolved+two+years+ago.+She+took+them+to+enjoy+the+euphoria+and+for+occasional+backache+or+tension+headaches.+Eventually%2c+she+found+she+had+to+take+it+all+the+time+to+avoid+feeling+%22flu-like.%22+%0a%0aAt+today%27s+office+visit%2c+Ms.+Small+contradicted+herself+during+history+taking.+To+make+sure+she+wasn%27t+trying+to+get+a+hydrocodone+prescription+for+pain+she+does+not+really+have%2c+she+was+confronted+about+the+contradiction.+She+admitted+this+was+the+case.+She+also+admitted+that+she+spends+too+much+money+and+time+obtaining+the+medication+illegally+and+that+she+wants+to+quit+but+cannot.+%0a%0aWith+just+this+much+information%2c+what+can+you+say+about+her+status+with+respect+to+developing+opioid+use+disorder%3f++(Choose+the+best+answer)</QuestionValue>
                <Image>6fbf0853de</Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>SummarySection/Post-TestTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Borderline+opioid+use+disorder</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Ms.+Small+meets+at+least+four+criteria%3a%0a1.+Taking+opioids+for+longer+than+intended%0a2.+Withdrawal+symptoms%0a3.+Spending+a+lot+of+time+obtaining+the+medication%0a4.+Wanting+to+quit+but+not+being+able+to+so.+%0aOnly+two+criteria+are+needed+for+a+diagnosis+of+opioid+use+disorder.</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Meets+enough+criteria+for+diagnosis+of+opioid+use+disorder</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Ms.+Small+meets+at+least+four+criteria%3a%0a1.+Taking+opioids+for+longer+than+intended%0a2.+Withdrawal+symptoms%0a3.+Spending+a+lot+of+time+obtaining+the+medication%0a4.+Wanting+to+quit+but+not+being+able+to+so.+%0aOnly+two+criteria+are+needed+for+a+diagnosis+of+opioid+use+disorder.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Does+not+meet+enough+criteria+for+diagnosis+of+opioid+use+disorder</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Ms.+Small+meets+at+least+four+criteria%3a%0a1.+Taking+opioids+for+longer+than+intended%0a2.+Withdrawal+symptoms%0a3.+Spending+a+lot+of+time+obtaining+the+medication%0a4.+Wanting+to+quit+but+not+being+able+to+so.+%0aOnly+two+criteria+are+needed+for+a+diagnosis+of+opioid+use+disorder.</FeedbackValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>Which+patients+being+prescribed+opioids+should+have+urine+drug+testing%3f+(Choose+the+best+answer)</QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>SummarySection/Post-TestTab/LabEntry: 1</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Only+patients+being+prescribed+high+dose+opioids</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Only+new+patients+being+prescribed+opioids+for+chronic+pain</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Only+patients+being+prescribed+any+opioids+who+have+aberrant+drug-related+behavior</OptionValue>
                        <OptionTypeValue>Incorrect</OptionTypeValue>
                        <FeedbackValue>Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</FeedbackValue>
                      </PanelData>
                    </Entry2>
                    <Entry3>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>Any+patient+being+prescribed+opioids+for+chronic+pain</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>Current+guidelines+for+chronic+opioid+therapy+published+by+the+CDC+(Dowell+et+al.%2c+2016)+recommend+baseline+urine+drug+testing+for+any+patient+being+prescribed+opioids+for+chronic+pain%2c+and+periodic+tests+thereafter.</FeedbackValue>
                      </PanelData>
                    </Entry3>
                  </EntryData>
                </data>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </QuizTab>
    </_SummarySection>
  </Sections>
</body>
";

    string legacyData = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<body>
  <SaveCaseBG>
    <PatientNameValue>
    </PatientNameValue>
    <PrivateToggle>True</PrivateToggle>
    <TemplateToggle>False</TemplateToggle>
    <DescriptionValue>
    </DescriptionValue>
    <SummaryValue>
    </SummaryValue>
    <TagsValue>
    </TagsValue>
    <TagsValue>
    </TagsValue>
    <TargetAudienceValue>MD%2fDO%2fPA%2fNP</TargetAudienceValue>
    <DifficultyValue>Intermediate</DifficultyValue>
  </SaveCaseBG>
  <CharacterEditorPanel>
    <CharacterNameValue>
    </CharacterNameValue>
    <BMIValue>%23%23%23+kg%2fm%c2%b2</BMIValue>
    <HeightValue>
    </HeightValue>
    <HeightSlider>42</HeightSlider>
    <WeightValue>
    </WeightValue>
    <WeightSlider>80</WeightSlider>
    <AgeValue>
    </AgeValue>
    <AgeSlider>13</AgeSlider>
    <SkinColor1Toggle>False</SkinColor1Toggle>
    <SkinColor2Toggle>False</SkinColor2Toggle>
    <SkinColor3Toggle>False</SkinColor3Toggle>
    <SkinColor4Toggle>False</SkinColor4Toggle>
    <SkinColor5Toggle>False</SkinColor5Toggle>
    <SkinColor6Toggle>False</SkinColor6Toggle>
    <SkinColor7Toggle>False</SkinColor7Toggle>
    <SkinColor8Toggle>False</SkinColor8Toggle>
    <SkinColor9Toggle>False</SkinColor9Toggle>
    <SkinColor10Toggle>False</SkinColor10Toggle>
    <SkinColor11Toggle>False</SkinColor11Toggle>
    <SkinColor12Toggle>False</SkinColor12Toggle>
    <Face1Toggle>True</Face1Toggle>
    <Face2Toggle>False</Face2Toggle>
    <Face3Toggle>False</Face3Toggle>
    <Face4Toggle>False</Face4Toggle>
    <Face5Toggle>False</Face5Toggle>
    <Face6Toggle>False</Face6Toggle>
    <Face7Toggle>False</Face7Toggle>
    <Face8Toggle>False</Face8Toggle>
    <Face9Toggle>False</Face9Toggle>
    <FaceSlider>1</FaceSlider>
    <HairColor1Toggle>False</HairColor1Toggle>
    <HairColor2Toggle>False</HairColor2Toggle>
    <HairColor3Toggle>False</HairColor3Toggle>
    <HairColor4Toggle>False</HairColor4Toggle>
    <HairColor5Toggle>False</HairColor5Toggle>
    <HairColor6Toggle>False</HairColor6Toggle>
    <HairColor7Toggle>False</HairColor7Toggle>
    <HairColor8Toggle>False</HairColor8Toggle>
    <HairColor9Toggle>False</HairColor9Toggle>
    <HairColor10Toggle>False</HairColor10Toggle>
    <HairColor11Toggle>False</HairColor11Toggle>
    <HairColor12Toggle>False</HairColor12Toggle>
    <HairSlider>1</HairSlider>
    <Hair1Toggle>True</Hair1Toggle>
    <Hair2Toggle>False</Hair2Toggle>
    <Hair3Toggle>False</Hair3Toggle>
    <Hair4Toggle>False</Hair4Toggle>
    <Hair5Toggle>False</Hair5Toggle>
    <Hair6Toggle>False</Hair6Toggle>
    <Hair7Toggle>False</Hair7Toggle>
    <Hair8Toggle>False</Hair8Toggle>
    <Hair9Toggle>False</Hair9Toggle>
  </CharacterEditorPanel>
  <Sections>
    <_Patient_IntroSection>
      <sectionName>
      </sectionName>
      <Personal_InfoTab>
        <customTabName>Personal Info</customTabName>
        <data>
          <EntryData>
            <Parent>Patient_IntroSection/Personal InfoTab/</Parent>
            <Entry0>
              <PanelType>BasicDetailsPanel</PanelType>
              <PanelData>
                <RecordValue>%23%23%23%23%23%23</RecordValue>
                <FirstNameValue>5</FirstNameValue>
                <LastNameValue>6</LastNameValue>
                <Gender>Female</Gender>
                <MonthValue>
                </MonthValue>
                <DayValue>
                </DayValue>
                <YearValue>
                </YearValue>
                <AgeValue>
                </AgeValue>
                <AllergiesValue>
                </AllergiesValue>
                <Education>Select</Education>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>AdditionalPanel</PanelType>
              <PanelData>
                <AdditionalInformationValue>
                </AdditionalInformationValue>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </Personal_InfoTab>
      <Office_VisitTab>
        <customTabName>Office Visit</customTabName>
        <data>
          <EntryData>
            <Parent>Patient_IntroSection/Office VisitTab/</Parent>
            <Entry0>
              <PanelType>OfficeVisitPanel</PanelType>
              <PanelData>
                <BPValue1>
                </BPValue1>
                <BPValue2>
                </BPValue2>
                <PulseValue>
                </PulseValue>
                <TempValue>
                </TempValue>
                <RespValue>
                </RespValue>
                <HeightValue>
                </HeightValue>
                <WeightValue>
                </WeightValue>
                <BMIValue>
                </BMIValue>
                <ChiefComplaintValue>
                </ChiefComplaintValue>
                <PainValue>N%2fA</PainValue>
                <HoPIValue>
                </HoPIValue>
              </PanelData>
            </Entry0>
          </EntryData>
        </data>
      </Office_VisitTab>
    </_Patient_IntroSection>
    <_stepSection>
      <sectionName>
      </sectionName>
      <Medication_HistoryTab>
        <customTabName>Medications</customTabName>
        <data>
          <EntryData>
            <Parent>stepSection/MedicationsTab/</Parent>
            <Entry0>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>a</MedicationValue>
                <DialoguePin>
                  <dialogue>
                    <uid>stepSection/MedicationsTab/LabEntry: 0</uid>
                    <characters>
                      <character>
                        <name>Patient</name>
                        <charColor>0.1058824,0.7215686,0.05882353</charColor>
                      </character>
                    </characters>
                    <data>
                      <EntryData>
                        <Parent>stepSection/MedicationsTab/LabEntry: 0</Parent>
                        <Entry0>
                          <PanelType>DialogueEntry</PanelType>
                          <PanelData id=""fcbdfc605c"">
                            <characterName>Patient</characterName>
                            <charColor>RGBA(0.106, 0.722, 0.059, 1.000)</charColor>
                            <dialogueText>1</dialogueText>
                          </PanelData>
                        </Entry0>
                        <Entry1>
                          <PanelType>DialogueEntry</PanelType>
                          <PanelData>
                            <characterName>Provider</characterName>
                            <charColor>RGBA(0.000, 0.251, 0.957, 1.000)</charColor>
                            <dialogueText>2</dialogueText>
                          </PanelData>
                        </Entry1>
                        <Entry2>
                          <PanelType>DialogueEntry</PanelType>
                          <PanelData>
                            <characterName>Instructor</characterName>
                            <charColor>RGBA(0.569, 0.569, 0.569, 1.000)</charColor>
                            <dialogueText>3</dialogueText>
                          </PanelData>
                        </Entry2>
                        <Entry3>
                          <PanelType>DialogueChoiceEntry</PanelType>
                          <PanelData>
                            <data>
                              <EntryData>
                                <Parent>stepSection/MedicationsTab/LabEntry: 0::LabEntry: 3</Parent>
                                <Entry0>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>a</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>a2</FeedbackValue>
                                  </PanelData>
                                </Entry0>
                                <Entry1>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>b</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>b2</FeedbackValue>
                                  </PanelData>
                                </Entry1>
                                <Entry2>
                                  <PanelType>QuizQuestionOption</PanelType>
                                  <PanelData>
                                    <OptionValue>c</OptionValue>
                                    <OptionTypeValue>Correct</OptionTypeValue>
                                    <FeedbackValue>c2</FeedbackValue>
                                  </PanelData>
                                </Entry2>
                              </EntryData>
                            </data>
                          </PanelData>
                        </Entry3>
                        <Entry4>
                          <PanelType>DialogueEntry</PanelType>
                          <PanelData>
                            <characterName>Instructor</characterName>
                            <charColor>RGBA(0.569, 0.569, 0.569, 1.000)</charColor>
                            <dialogueText>4</dialogueText>
                          </PanelData>
                        </Entry4>
                      </EntryData>
                    </data>
                  </dialogue>
                </DialoguePin>
                <QuizPin>
                  <data>
                    <EntryData>
                      <Parent>stepSection/MedicationsTab/LabEntry: 0</Parent>
                      <Entry0>
                        <PanelType>QuizQuestion</PanelType>
                        <PanelData>
                          <QuestionValue>qqq</QuestionValue>
                          <Image>
                          </Image>
                          <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                          <data>
                            <EntryData>
                              <Parent>stepSection/MedicationsTab/LabEntry: 0::LabEntry: 0</Parent>
                              <Entry0>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>11</OptionValue>
                                  <OptionTypeValue>Correct</OptionTypeValue>
                                  <FeedbackValue>12</FeedbackValue>
                                </PanelData>
                              </Entry0>
                              <Entry1>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>21</OptionValue>
                                  <OptionTypeValue>Partially+Correct</OptionTypeValue>
                                  <FeedbackValue>22</FeedbackValue>
                                </PanelData>
                              </Entry1>
                              <Entry2>
                                <PanelType>QuizQuestionOption</PanelType>
                                <PanelData>
                                  <OptionValue>31</OptionValue>
                                  <OptionTypeValue>Incorrect</OptionTypeValue>
                                  <FeedbackValue>32</FeedbackValue>
                                </PanelData>
                              </Entry2>
                            </EntryData>
                          </data>
                        </PanelData>
                      </Entry0>
                    </EntryData>
                  </data>
                </QuizPin>
                <StartMonthValue>1</StartMonthValue>
                <StartDayValue>2</StartDayValue>
                <StartYearValue>3333</StartYearValue>
                <EndMonthValue>
                </EndMonthValue>
                <EndDayValue>
                </EndDayValue>
                <EndYearValue>
                </EndYearValue>
                <DoseValue>b</DoseValue>
                <HowTakenValue>
                </HowTakenValue>
                <ConditionValue>
                </ConditionValue>
                <ResponseValue>
                </ResponseValue>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>
                </MedicationValue>
                <StartMonthValue>
                </StartMonthValue>
                <StartDayValue>
                </StartDayValue>
                <StartYearValue>
                </StartYearValue>
                <EndMonthValue>
                </EndMonthValue>
                <EndDayValue>
                </EndDayValue>
                <EndYearValue>
                </EndYearValue>
                <DoseValue>
                </DoseValue>
                <HowTakenValue>
                </HowTakenValue>
                <ConditionValue>
                </ConditionValue>
                <ResponseValue>
                </ResponseValue>
              </PanelData>
            </Entry1>
            <Entry2>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>
                </MedicationValue>
                <StartMonthValue>
                </StartMonthValue>
                <StartDayValue>
                </StartDayValue>
                <StartYearValue>
                </StartYearValue>
                <EndMonthValue>
                </EndMonthValue>
                <EndDayValue>
                </EndDayValue>
                <EndYearValue>
                </EndYearValue>
                <DoseValue>
                </DoseValue>
                <HowTakenValue>
                </HowTakenValue>
                <ConditionValue>
                </ConditionValue>
                <ResponseValue>
                </ResponseValue>
              </PanelData>
            </Entry2>
            <Entry3>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>
                </MedicationValue>
                <StartMonthValue>
                </StartMonthValue>
                <StartDayValue>
                </StartDayValue>
                <StartYearValue>
                </StartYearValue>
                <EndMonthValue>
                </EndMonthValue>
                <EndDayValue>
                </EndDayValue>
                <EndYearValue>
                </EndYearValue>
                <DoseValue>
                </DoseValue>
                <HowTakenValue>
                </HowTakenValue>
                <ConditionValue>
                </ConditionValue>
                <ResponseValue>
                </ResponseValue>
              </PanelData>
            </Entry3>
            <Entry4>
              <PanelType>MedicationEntryPanel</PanelType>
              <PanelData>
                <MedicationValue>
                </MedicationValue>
                <StartMonthValue>
                </StartMonthValue>
                <StartDayValue>
                </StartDayValue>
                <StartYearValue>
                </StartYearValue>
                <EndMonthValue>
                </EndMonthValue>
                <EndDayValue>
                </EndDayValue>
                <EndYearValue>
                </EndYearValue>
                <DoseValue>
                </DoseValue>
                <HowTakenValue>
                </HowTakenValue>
                <ConditionValue>
                </ConditionValue>
                <ResponseValue>
                </ResponseValue>
              </PanelData>
            </Entry4>
          </EntryData>
        </data>
      </Medication_HistoryTab>
      <QuizTab>
        <customTabName>Quiz</customTabName>
        <data>
          <EntryData>
            <Parent>stepSection/QuizTab/</Parent>
            <Entry0>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>question</QuestionValue>
                <Image>6babc27416</Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>stepSection/QuizTab/LabEntry: 0</Parent>
                    <Entry0>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>a</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>b</FeedbackValue>
                      </PanelData>
                    </Entry0>
                    <Entry1>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>c</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>d</FeedbackValue>
                      </PanelData>
                    </Entry1>
                    <Entry2>
                      <PanelType>QuizQuestionOption</PanelType>
                      <PanelData>
                        <OptionValue>e</OptionValue>
                        <OptionTypeValue>Correct</OptionTypeValue>
                        <FeedbackValue>f</FeedbackValue>
                      </PanelData>
                    </Entry2>
                  </EntryData>
                </data>
              </PanelData>
            </Entry0>
            <Entry1>
              <PanelType>QuizTabQuestion</PanelType>
              <PanelData>
                <QuestionValue>
                </QuestionValue>
                <Image>
                </Image>
                <OptionTypeValue>Multiple+Choice</OptionTypeValue>
                <data>
                  <EntryData>
                    <Parent>stepSection/QuizTab/LabEntry: 1</Parent>
                  </EntryData>
                </data>
              </PanelData>
            </Entry1>
          </EntryData>
        </data>
      </QuizTab>
    </_stepSection>
  </Sections>
</body>
";
}
