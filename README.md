# Deploy&Decrypt
# Note: WIP- 
Start date: May 08, 2023
Last updated July 06, 2023
Role: Developer, 2D Art creator (hand drawn and AI-generated)
## **Introduction**
![UnitAndBase](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/e7a3830d-899a-4d7e-bbb7-8d59a8cbb56d)

Welcome to the Tower Defense and RTS game demo! Defend your futuristic van base from enemy attacks using a cloning machine to create units and equip them with different weapons. Strategically deploy your units and unleash their abilities to emerge victorious! Enjoy the exciting gameplay and share your feedback with us.

## **Gameplay feature**
## **Unit**

In my game, each unit is rich in statistics and capabilities, enhancing the strategic depth of gameplay. Here's a brief overview of the unit features:

### **Stats:**

- Health and Max Health: Determines the unit's durability and ability to survive.
- Speed and Agility: Influences the unit's movement and responsiveness.
- Morale and Charisma: Affects the unit's morale and ability to inspire allies.
- Dodge Chance: Represents the unit's ability to evade incoming attacks.
- Close Range and Far Range: Specifies the unit's effectiveness in different combat distances.
- Accuracy: Reflects the unit's precision in hitting targets.
- Critical Chance and Critical Damage: Determines the likelihood and impact of critical hits.
- Melee Damage and Range Damage: Represents the unit's damage output in close-quarters and ranged attacks.
- Elemental Damage: Indicates the unit's ability to inflict different elemental damage types such as poison, fire, cryo, electrified, and explosion damage.
- Resistance: Describes the unit's defense against various types of damage, including bullets, melee attacks, poison, fire, cryo, electrified, and explosion damage.
  
### **Skills**
![Abilities](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/bd038a16-2b05-44ca-8189-35fa66c1d616)
- Active Skill: Unique abilities that can be actively triggered by the player during battles.
- Passive Skill: Inherent abilities that provide constant benefits to the unit.
- Support Skill: Skills that assist and boost nearby allied units.
All skills are made by using Unity's ScriptableObject:

Each unit type and weapon in the game is created using the same component, reducing the need for extra scripts.
Skills, statistics, and other attributes can be easily assigned and modified using ScriptableObject, ensuring modularity and ease of development.
By focusing on modularity, the game design allows for the creation of diverse and specialized units, providing players with various strategic options and gameplay possibilities.

Note: Please keep in mind that this is a simplified overview, and the actual implementation and mechanics may be more complex in the full game.
### **Targeting AI**
The targeting system of each unit in the game is dynamic and adaptable. Here are the different targeting options available to the units:

- SeekClosestEnemy: The unit will target the nearest enemy.
- SeekClosestAlly: The unit will target the closest ally.
- SeekClosestAndLowestHealthAlly: The unit will target the closest ally with the lowest health.
- SeekClosestAndLowestHealthEnemy: The unit will target the nearest enemy with the lowest health.
- SeekClosestAttackerAlly: The unit will target the closest ally being attacked.
- SeekClosestAttackerEnemy: The unit will target the nearest enemy attacking an ally.
- SeekClosestAndHighestHealthAlly: The unit will target the closest ally with the highest health.
- SeekClosestAndHighestHealthEnemy: The unit will target the nearest enemy with the highest health.
By utilizing these different targeting options, units can dynamically select their targets based on specific criteria, such as proximity, health status, or the role of the target (enemy or ally). This flexible targeting system allows for strategic decision-making and adaptability during battles.
  
### **State pattern**
![State1](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/cfc89f0f-8960-444d-bb74-65570d1f148f)

![State2](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/caec4069-305c-472a-9488-8cf5c993c381)
In the game, the State Pattern is implemented to organize and maintain the behaviors of each unit more effectively. Each unit, whether categorized as an Attacker or Supporter type, has its own state machine.

Let's explore the different states a unit can have:

- Idle: This state represents when the unit is not engaged in any specific action and remains stationary.
- Moving: Units enter this state when they need to move to a designated location, actively navigating the game world.
- Range Attacking: Units in this state perform ranged attacks, dealing damage from a distance. If ammunition runs out, they transition to the Reload state to replenish their supply.
- Melee Attack: This state signifies the unit's engagement in close-quarters combat, delivering powerful attacks to their targets.
- Support: Support-type units focus on providing assistance to allies. While in the Support state, they can heal or buff teammates.
- Using Passive Ability: Units with passive abilities continuously benefit from them while in this state. These abilities are always active.
- Using Active Ability: This state enables units to activate their active abilities, which have specific effects and can be triggered by the player.
- Death: When a unit is defeated or destroyed, it transitions to the Death state.
The State Pattern enhances the organization and maintainability of the codebase. Each state encapsulates a specific behavior, allowing for easy modification or extension without impacting other states or introducing complex conditional logic.

By employing the State Pattern, the codebase achieves improved readability and flexibility in defining unit behaviors. It enables efficient management of different unit types' states, resulting in a more immersive and enjoyable gameplay experience.
## **Environment**

In the game, the background and most assets are created using AI-generated content to avoid copyright issues. This approach ensures that the game's visuals are unique and original, reducing the risk of infringing on existing copyrighted material.
![Below Road(Upscaled)](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/c7e9425e-5a8f-42e7-9e1d-6d44c8123ed8)
![Layer 2](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/9d831750-d623-4d04-931b-122f7bb48ad5)
![Layer 3(Upscaled)](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/2997f7e8-5dbf-499f-89e6-9fe5f1ae32ca)
![Layer 4](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/19a10734-2f80-4db4-a3c8-5198a20a18fc)
![Layer1(Upscaled)](https://github.com/AkaiNoval/Unity-Radio-Frequency/assets/127651185/b06d14ff-a0b3-480f-b36d-4de0aa247f3d)
Additionally, the game incorporates a Parallax effect to create a sense of depth and movement in the background. The Parallax effect involves displaying multiple layers of images at different speeds, giving the illusion of depth as the player moves through the game environment. This technique enhances the visual appeal of the game and adds to the immersive experience.
## **Player Control**
The game is designed for mobile devices, allowing players to control the gameplay using touch controls. One finger is used for movement, while two fingers control zooming in and out. The controls are intuitive and optimized for a seamless mobile gaming experience.
