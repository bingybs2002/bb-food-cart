# Gacha API
![Gacha vending machine](https://pngtree.com/so/gashapon-machine)

This is an api that serves as the reward system for the bb-foodtruck. As customer buys more and more stuff, we have this reward system that allows the customer to Gacha their luck and win prices. For every 25 dollar spent, customer can have 1 Roll in Gacha.
### Drop Rate on Count of 30
| Rarity     | Expected Rate | Count (of 30) | Actual Rate(%) |
|------------|---------------|---------------|-------------|
| Common     | 50%           | 15            | 50%         |
| Rare       | 30%           | 9             | 30%         |
| Epic       | 12%           | 3             | 10%         |
| Mystic     | 7%            | 2             | 6.66%       |
| Legendary  | 1%            | 1             | 3.33%       |

# API endpoints:
### Gacha Item Management
1. POST
    - /Gacha: Insert one entry of Gacha Item in JSON
    - /Gacha/bulk: Insert many entries of Gacha Items in JSON
2. GET
    - /: Homepage and instructions
    - /Gacha: View all entries
3. Delete
    - /gacha/{id}: Delete the Gacha Item by Id.
    - /gacha/reset: Remove all Gacha Items.