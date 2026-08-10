# 📊 Smart Expense Analyzer

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20Onion-blue?style=for-the-badge)](#-architectural-overview)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

A robust, enterprise-grade Smart Expense Analyzer application built using **C# 12** and the **.NET 8** framework. Designed with strict adherence to **Clean Architecture (Onion Architecture)** principles, this project ensures high maintainability, domain isolation, and comprehensive testability. It serves as a core demonstration of production-level software design patterns within an undergraduate technical portfolio.

---

## 👨‍💻 Developer Profile

* **Author:** Semila Amajith  
* **Role:** Undergraduate  
* **Department:** Department of Information & Communication Technology (BICT Hons)  
* **Institution:** Faculty of Technology, University of Sri Jayewardenepura, Sri Lanka  
* **GitHub:** [@SemilaAmajith2004](https://github.com/SemilaAmajith2004)  
* **LinkedIn:** [Semila Amajith](https://www.linkedin.com/in/semila-amajith/)

---

## 🚀 Core Features

* **👤 User Management Architecture:** Implements secure user profile structures and isolated data boundaries.
* **💸 Expense Tracking Engine:** Efficient calculation, categorization, and management of data streams.
* **💾 Infrastructure Persistence:** Optimized local data storage framework for file-bound transactions.
* **📊 Structured UI Layer:** Clean desktop-based visualization dashboard built on C# presentation layers.
* **🧪 Automated Quality Assurance:** Dedicated unit testing infrastructure verifying critical business rules.

---

## 🏗️ Architectural Overview

The solution architecture separates concerns across highly decoupled operational layers. Business rules remain independent of UI logic, external frameworks, and database dependencies.

```text
Smart-Expense-Analyzer/
│
├── 📁 SmartExpenseAnalyzer.Core            # Domain Models, Use Cases, and Core Contracts
├── 📁 SmartExpenseAnalyzer.Infrastructure    # Data Persistence, File I/O, and Implementations
├── 📁 SmartExpenseAnalyzer.Shared            # Common Data Transfer Objects (DTOs) and Utilities
├── 📁 SmartExpenseAnalyzer.UI                # Desktop Presentation Interface (Dashboard)
└── 📁 SmartExpenseAnalyzer.Tests             # Isolated Unit Test Suites
