import json 
a = 1 
b = "Example Item" 
c = None 
d = 0 
items = [] 
for i in range(30):
    data = { "Id": a + i, "ItemName": b, "Description": c, "ItemRarity": d } 
    items.append(data) 

with open("data.json", "w") as f: 
    json.dump(items, f, indent=4) 
    print("data is okay")