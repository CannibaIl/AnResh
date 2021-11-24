Use [AnReshProbation]
CREATE TABLE Users
(
Id INT IDENTITY (1, 1) PRIMARY KEY,
EmployeeId INT NOT NULL,
Email VARCHAR(320) UNIQUE,
HasEmailConfirm BIT NOT NULL,
HashPassword VARBINARY(256) NOT NULL,
SaltPassword VARBINARY(50) NOT NULL,
Role VARCHAR(64) NOT NULL,
FOREIGN KEY (EmployeeId) REFERENCES Employees (Id)
)


INSERT Users(EmployeeId, Email, HasEmailConfirm, HashPassword, SaltPassword, Role) VALUES
(1,'admin@admin.admin', 1,
0xAD42C7AA734ACEE57A540043862CB650DA9316899F29DFEDEB27B4011C05227D3DB14F6C8510429068A2F92D97881FD2AE616B3923261AD9E0120C5E0A7A6F8F814FB1CD92B3844E3AD4960237E64B4968C3BE38E10EB8AB6FABC16B1D43A60D4BCD673C76303A7AC145AB9F3F8672B00CBFFEF2BF1A5850371DE19F62963FB2D92AA6E93360C1C73A62DEE10D2FAA6F94748A50E1CBA0819F9ABC0FB7D5304F665A3C722CAA4C24CC7110EBA4520276A944B147E7076A3922AE505A58A94FC7260BB427270CC4446978992B228E370EE8FC7932A847F1C557B9CB158BEB8A931C363A5FB2E4F8B08387E7F6E4DF88450E50ED3D4545A9C53A0AE9244EB97C09,
0x09E2573E65EE5E10E2443B3E979FEAE141EE2C75BE8554,
'Admin'),

(2,'user@user.user', 1,
0x0F88570B8447BFB15FC3DA47E8D3BDFDD1DA740D94B007D02E103A17D9755279CD44CE29E9184AD96A9114545619F95501DC47CC143DA1AAB4B73FC77F90EF4199D34B5EE7780F7C6E093F355D1C2B47C20958480FACEE45FD0DDD0630286D5496F15C69E7F3125BFAC5A7140BC10CA449A41E897710965C025388D34A921225CC97B8DC10C270554F57D2D81AA3C30961D1706C88249F6720137A30DC36A59D8DB398502AF6C1F84EFD8EA76CDA5F77A2DD759DA2E7182947CB2D4DE1C95F41B6408562B643527D6121BAFAAADEDE0E80CD0A085AA6F9FBDA4EA14630C486CD210B2950E8C4882A2F9D7089B01388620B7A98F918F33372D07614ACDBCA3F7B,
0xC8A5B49FB0FF3A91FD148435B7E325B6CD1682B86A79255BB1A1C6F2E29436F648642CEB023671ECF12152380213CD5C,
'User'),

(4,'Jerry@gmaill.com', 1,
0x0347632454BACE2F4C75CA8F08F02DF7D2A29AF3ECD87BA58971D4E75B9BCEAEB53F28B356DCEA66138F26E11A33D2C31861F27F53E7A5E0B58AE578382C424845074F9AB5EA8C4627E3F47C250B3793863F6917E98B916716CB2FF8C1DFC2D14A7D884FB3A7F453A0263F31B40A72625B25252B75C541F7306C95CD1C7F5DC81718A3F173F5AFC4D1691346DE56E55A366E5945E54C52657AFE424FFF095A08B79C7CAC7B3E2135F036DB2402693015E560E7129B3689F366C3818ACB5E9D50A6BADAB709BA1ABD2DBDBED4D54CB5F2D668BA9A1943F531C74DEA72B40F911144EF6059CB663A5A3E27E8F23E0549891F963A445F5414EBBE89784201F35F47,
0x0FB21D989B8B940BCA71CFDA10406613BDBA2C1C7A0D5F6F94C2423BDBECCEB1E77404645D2B6F93ED997CBD,
'User');