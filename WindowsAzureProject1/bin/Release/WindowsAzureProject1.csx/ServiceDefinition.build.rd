<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WindowsAzureProject1" generation="1" functional="0" release="0" Id="8a1abbf8-7d06-4264-9506-5aa9dc4b4241" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WindowsAzureProject1Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SETSReport:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/LB:SETSReport:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="SETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/LB:SETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapCertificate|SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
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
        <aCS name="SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="SETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
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
        <lBChannel name="LB:SETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
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
        <map name="MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapSETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
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
          <role name="SETSReport" generation="1" functional="0" release="0" software="C:\VUE\MvcApplication3\WindowsAzureProject1\bin\Release\WindowsAzureProject1.csx\roles\SETSReport" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="8080" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SW:SETSReport:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SETSReport&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SETSReport&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
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
    <implementation Id="9577b47a-2846-4ff6-8fb1-b1244cd29784" ref="Microsoft.RedDog.Contract\ServiceContract\WindowsAzureProject1Contract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="c4165c5d-a508-49f9-8e47-fba8417ec861" ref="Microsoft.RedDog.Contract\Interface\SETSReport:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="5473de6d-ae7f-45f5-8399-d4b27cf16e82" ref="Microsoft.RedDog.Contract\Interface\SETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/WindowsAzureProject1/WindowsAzureProject1Group/SETSReport:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>