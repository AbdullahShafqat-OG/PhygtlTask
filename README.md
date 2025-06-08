# PhygtlTask

## 🛠 Development Approach

The project uses a **two-scene structure**:

- **Level Select Scene**: Displays available levels and allows the player to choose one.
- **Level Scene**: A single, reusable scene that handles the match-3 gameplay for all levels.

Level configurations are stored in **ScriptableObjects**. These assets act as both data containers and editable level templates, enabling designers to create or update levels directly in the Unity Editor.

Using **one shared Level Scene** for all levels ensures that gameplay-related changes are centralized. This makes it easier for designers to iterate quickly, as updates only need to be made in a single scene rather than across multiple level scenes.

---

## 📡 UI Communication

For communication between gameplay systems and the UI, the **Observer pattern** is used via a custom `Messenger` class. This lightweight messaging system allows different components  to subscribe to and react to relevant game events without tight coupling.

This approach improves code modularity, simplifies event management, and makes the UI system easier to extend or refactor.

---

## ⚙️ Game Architecture

The game logic is structured to **separate core functionality from visual representation**. The internal mechanics—such as grid management, matching logic, and tile state—are decoupled from visuals and animations.

This design enables easy swapping or updating of the game's look and feel without altering gameplay systems. It supports flexibility, better testing, and scalability for future improvements.

---

## 🚀 Code Structure & Performance

- The codebase is **modular and decoupled**, following clean architecture principles.
- **Object pooling** is used for tiles and effects to minimize runtime instantiations.
- A **state-driven approach** ensures that only necessary processing occurs, improving performance and responsiveness.

---

## 🧪 Testing the Build

- Open the project in Unity 2022.3+
- Load `MainLevel` scene in `Assets/_Project/Scenes` and hit Play
- Build the APK and deploy to an Android device