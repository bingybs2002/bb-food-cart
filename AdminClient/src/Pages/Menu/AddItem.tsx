import { AppSidebar } from "@/components/app-sidebar"
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { DataTable } from "./ViewMenu/data-table"
import { columns, type MenuItems } from "./ViewMenu/columns"
import { useEffect, useState } from "react"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"

const API = import.meta.env.VITE_API_BASE_URL

export default function AddItem() {0
  const [Menu, SetMenu] = useState<MenuItems[]>([])

  const [addItemForm, setAddItemForm] = useState({
    name: "",
    description: "",
    allergies: 0,
    foodType: 0,
    isSoldOut: false,
    calories: 0,
    protein: 0,
    carbs: 0,
  })
  
  const loadMenu = async () => {
    const res = await fetch(`${API}/itemAdmin/Read`)
    const data: MenuItems[] = await res.json()
    SetMenu(data)
  }

  useEffect(() => {
    loadMenu()
  }, [])

  const postItem = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()

    try {
      const response = await fetch(`${API}/itemAdmin/Create`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
        },
        body: JSON.stringify([addItemForm]),
      })

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      await loadMenu()

      setAddItemForm({
        name: "",
        description: "",
        allergies: 0,
        foodType: 0,
        isSoldOut: false,
        calories: 0,
        protein: 0,
        carbs: 0,
      })
    } catch (error) {
      console.error("Error:", error)
    }
  }

  return (
    <SidebarProvider>
      <AppSidebar />

      <main className="relative w-full space-y-6 p-6">
        <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">
          BB Food Cart | Add A New Item
        </h1>

        <div className="container mx-auto py-10">
          <Card>
            <CardHeader>
              <CardTitle>Add Item</CardTitle>
              <CardDescription>Add item to the menu</CardDescription>
            </CardHeader>

            <form onSubmit={postItem}>
              <CardContent>
                <div className="flex flex-col gap-6">
                  <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
                    <div className="grid gap-2 md:col-span-2">
                      <Label>Item Name</Label>
                      <Input
                        value={addItemForm.name}
                        onChange={(e) =>
                          setAddItemForm({
                            ...addItemForm,
                            name: e.target.value,
                          })
                        }
                        required
                      />
                    </div>

                    <div className="grid gap-2">
                      <Label>Food Type</Label>
                      <Input
                        type="number"
                        value={addItemForm.foodType}
                        onChange={(e) =>
                          setAddItemForm({
                            ...addItemForm,
                            foodType: Number(e.target.value),
                          })
                        }
                        required
                      />
                    </div>
                  </div>

                  <div className="grid gap-2">
                    <Label>Description</Label>
                    <Textarea
                      value={addItemForm.description}
                      onChange={(e) =>
                        setAddItemForm({
                          ...addItemForm,
                          description: e.target.value,
                        })
                      }
                      className="min-h-[120px]"
                      required
                    />
                  </div>

                  <div className="grid grid-cols-1 gap-4 md:grid-cols-4">
                    <div className="grid gap-2">
                      <Label>Allergies</Label>
                      <Input
                        type="number"
                        value={addItemForm.allergies}
                        onChange={(e) =>
                          setAddItemForm({
                            ...addItemForm,
                            allergies: Number(e.target.value),
                          })
                        }
                        required
                      />
                    </div>

                    <div className="grid gap-2">
                      <Label>Calories</Label>
                      <Input
                        type="number"
                        value={addItemForm.calories}
                        onChange={(e) =>
                          setAddItemForm({
                            ...addItemForm,
                            calories: Number(e.target.value),
                          })
                        }
                        required
                      />
                    </div>

                    <div className="grid gap-2">
                      <Label>Protein</Label>
                      <Input
                        type="number"
                        value={addItemForm.protein}
                        onChange={(e) =>
                          setAddItemForm({
                            ...addItemForm,
                            protein: Number(e.target.value),
                          })
                        }
                        required
                      />
                    </div>

                    <div className="grid gap-2">
                      <Label>Carbs</Label>
                      <Input
                        type="number"
                        value={addItemForm.carbs}
                        onChange={(e) =>
                          setAddItemForm({
                            ...addItemForm,
                            carbs: Number(e.target.value),
                          })
                        }
                        required
                      />
                    </div>
                  </div>
                </div>
              </CardContent>

              <CardFooter>
                <Button type="submit" variant="outline" size="sm" className="w-full">
                  Add Item
                </Button>
              </CardFooter>
            </form>
          </Card>
        </div>

        <div className="container mx-auto py-10">
          <DataTable columns={columns} data={Menu} />
        </div>

        <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}