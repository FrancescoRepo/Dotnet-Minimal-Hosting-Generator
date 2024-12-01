
### **Dotnet Minimal Hosting Model Generator**

---

#### **Description**
`DotnetMinimalHostingModelGenerator` is a command-line tool designed to help developers transition their ASP.NET Core applications to the modern *minimal hosting model*. This tool reads a traditional `Startup.cs` file and generates a new `Program.cs` file that adheres to the minimal hosting model introduced in .NET 6 and later.

---

#### **Features**
- **Transform `Startup.cs`:** Converts the contents of a traditional `Startup.cs` file to a minimal hosting model format.
- **Customizable Output:** Allows specifying the directory where the new `Program.cs` file will be saved.
- **Validation:** Ensures the specified `Startup.cs` file and output directory exist before proceeding.

---

#### **Installation**

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd DotnetMinimalHostingModelGenerator
   ```

2. **Build the Tool**
   ```bash
   dotnet build
   ```

3. **Install as a Global Tool (Optional)**
   ```bash
   dotnet pack
   dotnet tool install --global --add-source ./nupkg DotnetMinimalHostingModelGenerator
   ```
   Or launch the script "tool_install.sh" available in the folder.

---

#### **Usage**

1. Open a terminal.
2. Run the tool with the following syntax (if installed globally):

   ```bash
   minimal-hm-generator transform --startup-file <path-to-Startup.cs> --output-file-path <output-directory>
   ```

   Example:
   ```bash
   minimal-hm-generator transform --startup-file ./Startup.cs --output-file-path ./Output/
   ```

---

#### **Options**

| Option                   | Description                                      | Example                                 |
|--------------------------|--------------------------------------------------|-----------------------------------------|
| `--startup-file`         | The path to the `Startup.cs` file to transform.  | `--startup-file ./Startup.cs`          |
| `--output-file-path`     | The directory where `Program.cs` will be saved.  | `--output-file-path ./Output/`         |

---

#### **Validation Rules**
- The specified `Startup.cs` file must exist. Otherwise, an error will be shown:
  ```plaintext
  Startup file './Startup.cs' does not exist.
  ```
- The specified output directory must exist. Otherwise, an error will be shown:
  ```plaintext
  Output file path './Output/' does not exist.
  ```

---

#### **Example Output**

Running the tool successfully will output a message like:
```plaintext
Successfully transformed ./Startup.cs
```

The generated file will be called "NewProgram.cs" and will be located in the specified output directory.

---

#### **Exit Codes**
- **0:** Successful operation.
- **1:** Errors during validation or transformation.

---

#### **Contributing**
Contributions are welcome! Please submit issues or pull requests to improve the tool.

---

#### **License**
This tool is open source and distributed under the MIT License. See the `LICENSE` file for details.
