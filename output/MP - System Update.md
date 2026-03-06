# MP - System Update - Process Flow Diagram

## Main Flow

```mermaid
flowchart TD
    Start([Start]) --> Init1[Initialize MyText]
    
    subgraph Microsoft_Store [Microsoft Store]
        direction TB
        MS_Launch[Launch]
        MS_Start[Start_Updates]
        MS_Wait[Wait_Updates_Finished]
        MS_Term[Terminate]
    end
    
    subgraph Windows_Settings [Windows Settings]
        direction TB
        WS_Launch[Launch]
        WS_Start[Start_Updates]
        WS_Wait[Wait_Updates_Finished]
        WS_Term[Terminate]
    end
    
    Init1 --> MS_Launch
    MS_Launch --> MS_Start
    MS_Start --> WS_Launch
    WS_Launch --> WS_Start
    
    %% Both apps running in parallel
    WS_Start -->|"Both apps running"| MS_Wait
    MS_Wait --> MS_Term
    MS_Term --> WS_Wait
    WS_Wait --> WS_Term
    
    WS_Term --> Subprocess[Call MP_Subprocess_A]
    Subprocess --> End([End])
    
    %% Styling
    classDef init fill:#fff2cc,stroke:#d6b656;
    classDef ms fill:#dae8fc,stroke:#6c8ebf;
    classDef ws fill:#e1d5e7,stroke:#9673a6;
    classDef sub fill:#f8cecc,stroke:#b85450;
    classDef startEnd fill:#d5e8d4,stroke:#82b366;
    
    class Init1 init;
    class MS_Launch,MS_Start,MS_Wait,MS_Term ms;
    class WS_Launch,WS_Start,WS_Wait,WS_Term ws;
    class Subprocess sub;
    class Start,End startEnd;
```

## Dummy Page (Not called in Main)

```mermaid
flowchart LR
    Start_dummy([Start]) --> Init_VNR[Initialize VNR]
    Init_VNR --> Action[bp_demo.MyPublicAction]
    Action --> End_dummy([End])
    
    classDef init fill:#fff2cc,stroke:#d6b656;
    classDef action fill:#f8cecc,stroke:#b85450;
    classDef startEnd fill:#d5e8d4,stroke:#82b366;
    
    class Init_VNR init;
    class Action action;
    class Start_dummy,End_dummy startEnd;
```

## Variable_Test Page (Not called in Main) - GoTo Logic

```mermaid
flowchart TD
    Start_var([Start]) --> Init_vars[Initialize variables]
    Init_vars --> Input[Input assignments]
    Input --> Decision{"MyToggle?"}
    
    Decision -->|True| Calc[GoTo Calculation]
    Decision -->|False| MultiCalc[GoTo MultipleCalc]
    
    Calc --> Output[Output assignments]
    MultiCalc --> Output
    
    Output --> End_var([End])
    
    classDef init fill:#fff2cc,stroke:#d6b656;
    classDef decision fill:#ffe6cc,stroke:#d79b00;
    classDef truePath fill:#dae8fc,stroke:#6c8ebf;
    classDef falsePath fill:#e1d5e7,stroke:#9673a6;
    classDef startEnd fill:#d5e8d4,stroke:#82b366;
    
    class Init_vars,Input,Output init;
    class Decision decision;
    class Calc truePath;
    class MultiCalc falsePath;
    class Start_var,End_var startEnd;
```

---

## Process Description

### Main Flow
The main process:
1. Initializes `MyText = "Hallo Welt"`
2. Launches Microsoft Store, starts updates
3. Launches Windows Settings, starts updates
4. Waits for Microsoft Store updates to finish, then terminates
5. Waits for Windows Settings updates to finish, then terminates
6. Calls MP_Subprocess_A with the MyText parameter
7. Ends

### Dummy Page (Not called in Main)
- Initializes `VNR = "AB123456"`
- Calls bp_demo.MyPublicAction

### Variable_Test Page (Not called in Main)
- Demonstrates GoTo-based flow control
- Uses static MyToggle variable that retains value between calls
- Conditional branching with labels (Calculation vs MultipleCalc)

---

**Note:** This page demonstrates GoTo-based flow control using conditional branching with labels. The MyToggle variable is static, meaning it retains its value between calls.