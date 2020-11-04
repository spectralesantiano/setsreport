<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WindowsAzureProject1" generation="1" functional="0" release="0" Id="6139f32d-61ef-4514-9ace-e6f6f5821b95" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WindowsAzureProject1Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SETSReport:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/LB:SETSReport:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="SETSReport:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="SETSReport:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="SETSReport:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="SETSReport:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SETSReportInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReportInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:SETSReport:Endpoint1">
          <toPorts>
            <inPortMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapSETSReport:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapSETSReport:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapSETSReport:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapSETSReport:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSETSReportInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReportInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SETSReport" generation="1" functional="0" release="0" software="C:\VUE\MvcApplication3\WindowsAzureProject1\bin\Debug\WindowsAzureProject1.csx\roles\SETSReport" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SETSReport&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SETSReport&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReportInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="SETSReportInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e28b13a8-962d-4635-bd62-9695636deba6" ref="Microsoft.RedDog.Contract\ServiceContract\WindowsAzureProject1Contract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="76823152-f24f-4ed6-9af1-0f8f0b137507" ref="Microsoft.RedDog.Contract\Interface\SETSReport:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>