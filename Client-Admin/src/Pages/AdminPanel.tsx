import React, { useEffect, useState } from "react"
import { AppSidebar } from "@/components/app-sidebar"
import { Calendar } from "@/components/ui/calendar"
import { SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar"
import { Bar, BarChart, CartesianGrid, XAxis } from "recharts"
import {
  ChartContainer,
  ChartLegend,
  ChartLegendContent,
  ChartTooltip,
  ChartTooltipContent,
  type ChartConfig,
} from "@/components/ui/chart"

const API = import.meta.env.VITE_API_BASE_URL

type SalesDay = {
  day: string
  transactions: number
}

type PopularItemtype={
  name: string
  calories: number
  carbs: number
  protein: number
  totalSold: number
}

export default function HomePage() {
  const [date, setDate] = useState<Date | undefined>(new Date())
  const [salesToday, setSalesToday] = useState(0)
  const [salesIn7Days, setSalesIn7Days] = useState(0)
  const [popularItem, setPopularItem] = useState<PopularItemtype | null>(null)
  const [SoldOutItems, setSoldOutItems] = useState<string[]>([])
  const today = new Date()

  const chartConfig = {
    Visits: {
      label: "Visits",
      color: "#2563eb",
    },
    Orders: {
      label: "Orders",
      color: "#60a5fa",
    },
  } satisfies ChartConfig

  const chartData = Array.from({ length: 7 }, (_, i) => {
    const day = new Date()
    day.setDate(day.getDate() - (6 - i))

    return {
      month: day.toLocaleDateString("en-US", { weekday: "short" }),
      Visits: Math.floor(Math.random() * 300),
      Orders: Math.floor(Math.random() * 200),
    }
  })

  useEffect(() => {
    fetch(`${API}/Stat/salesToday`)
      .then((res) => res.json())
      .then((data) => setSalesToday(data))
  }, [])

  useEffect(() => {
    fetch(`${API}/Stat/salesInLast7Days`)
      .then((res) => res.json())
      .then((data)=>setSalesIn7Days(data))
  }, [])

useEffect(() => {
  fetch(`${API}/Stat/MostPopularItem`)
    .then((res) => {
      if (!res.ok) throw new Error("Failed to fetch most popular item")
      return res.json()
    })
    .then((data) => setPopularItem(data))
    .catch((err) => console.error(err))
}, [])

  useEffect(() => {
    fetch(`${API}/Stat/SoldOutItems`)
      .then((res) => res.json())
      .then((data)=>setSoldOutItems(data))
  }, [])
  return (
    <SidebarProvider>
      <AppSidebar />

      <main className="relative w-full p-6">
        <h1 className="mt-4 text-center text-4xl font-bold">
          Admin Dashboard | BB Food Cart
        </h1>

        <div className="mt-6 flex justify-end">
          <div className="w-60 rounded-lg border-2 border-red-500 p-4">
            <h3 className="text-center font-semibold">Our Cart Address:</h3>
            <p className="text-right text-stone-100">
              7350 Bustleton Avenue,
              <br />
              Philadelphia, 19149
            </p>
          </div>
        </div>
          
        <h6 className="text-lg font-bold text-heading text-center">Sales & Statistics</h6>
        <div className="mt-6 grid grid-cols-1 gap-6 lg:grid-cols-3">
          <div className="rounded-xl border-2 border-white p-4">
            <p className="mb-4 text-center font-semibold">Week Performance</p>
            <ChartContainer config={chartConfig} className="min-h-[200px] w-full">
              <BarChart accessibilityLayer data={chartData}>
                <CartesianGrid vertical={false} />
                <XAxis
                  dataKey="month"
                  tickLine={false}
                  tickMargin={10}
                  axisLine={false}
                  tickFormatter={(value) => value.slice(0, 3)}
                />
                <ChartTooltip content={<ChartTooltipContent />} />
                <ChartLegend content={<ChartLegendContent />} />
                <Bar dataKey="Visits" fill="var(--color-Visits)" radius={4} />
                <Bar dataKey="Orders" fill="var(--color-Orders)" radius={4} />
              </BarChart>
            </ChartContainer>
          </div>

          <div className="flex justify-center rounded-xl border-2 border-white p-4">
            <div>
          <h1 className="text-center">{today.toDateString()}</h1>
            <Calendar
              mode="single"
              selected={date}
              onSelect={setDate}
              className="rounded-lg border mt-2"
            />
            </div>
          </div>

          <div className="flex flex-col gap-4 items-start">
            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md h-full">
              <h2 className="text-lg text-gray-500">Sales Today</h2>
              <p className="mt-2 text-3xl font-bold">{salesToday}</p>
            </div>

            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md h-full">
              <h2 className="text-lg text-gray-500">Sales Last Week</h2>
              <p className="mt-2 text-3xl font-bold">{salesIn7Days}</p>
            </div>
          </div>
        </div>

        <h6 className="text-lg font-bold text-heading text-center mt-5">
          Menu and Items
        </h6>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mt-6">
          <div className="rounded-xl border-2 border-white p-4 w-full">
            <p className="mb-4 text-center font-semibold">Sold Out Items</p>

            {SoldOutItems.length > 0 ? (
              <ul className="space-y-1">
                {SoldOutItems.map((item) => (
                  <li key={item}>{item}</li>
                ))}
              </ul>
            ) : (
              <p>No sold out items</p>
            )}
          </div>

          <div className="rounded-xl border-2 border-white p-4 w-full">
            <p className="mb-4 text-center font-semibold">Most Popular Items</p>
              {popularItem ? (
                <div className="space-y-1">
                  <p>Name: {popularItem.name}</p>
                  <p>Total Sold: {popularItem.totalSold}</p>
                  <p>Calories: {popularItem.calories}</p>
                  <p>Carbs: {popularItem.carbs}</p>
                  <p>Protein: {popularItem.protein}</p>
                </div>
              ) : (
                <p>No data</p>
              )}
          </div>
        </div>


      <div className="mt-10">
         <h6 className="text-lg font-bold text-heading text-center mt-5">
        Gacha Statistics
        </h6>


      </div>


        <SidebarTrigger className="absolute bottom-5 ml-auto h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}