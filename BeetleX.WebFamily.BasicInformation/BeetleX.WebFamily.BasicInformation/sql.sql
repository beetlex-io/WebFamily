CREATE TABLE `departments` (
  `ID` varchar(45) NOT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `Note` varchar(500) DEFAULT NULL,
  `ManagerID` varchar(45) DEFAULT NULL,
  `SuperiorID` varchar(45) DEFAULT NULL,
  `SystemData` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `permissions` (
  `ID` varchar(45) NOT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `Code` varchar(500) DEFAULT NULL,
  `SystemData` int(11) DEFAULT NULL,
  `Note` varchar(500) DEFAULT NULL,
  `Category` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Code_UNIQUE` (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `properties` (
  `ID` varchar(45) NOT NULL,
  `Category` varchar(45) DEFAULT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `Value` varchar(45) DEFAULT NULL,
  `OrderValue` int(11) DEFAULT NULL,
  `SystemData` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `roles` (
  `ID` varchar(45) NOT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `Note` varchar(500) DEFAULT NULL,
  `SystemData` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `rolepermissions` (
  `RoleID` varchar(45) NOT NULL,
  `PermissionsID` varchar(45) NOT NULL,
  `Value` int(11) DEFAULT NULL,
  PRIMARY KEY (`RoleID`,`PermissionsID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `users` (
  `ID` varchar(45) NOT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `WorkNumber` varchar(45) DEFAULT NULL,
  `JobPostion` varchar(45) DEFAULT NULL,
  `DepartmentID` varchar(45) DEFAULT NULL,
  `EMail` varchar(45) DEFAULT NULL,
  `Note` varchar(500) DEFAULT NULL,
  `TelePhone` varchar(45) DEFAULT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `MobilePhone` varchar(45) DEFAULT NULL,
  `BirthDay` date DEFAULT NULL,
  `EntryDay` date DEFAULT NULL,
  `SuperiorID` varchar(45) DEFAULT NULL,
  `LoginPassword` varchar(45) DEFAULT NULL,
  `Enabled` int(11) DEFAULT NULL,
  `Icon` varchar(45) DEFAULT NULL,
  `SystemData` int(11) DEFAULT NULL,
  `Sex` varchar(10) DEFAULT NULL,
  `WeiXinID` varchar(50) DEFAULT NULL,
  `Roles` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `EMail_UNIQUE` (`EMail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;