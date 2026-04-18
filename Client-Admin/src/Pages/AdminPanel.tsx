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

type SalesDay = {
  day: string
  transactions: number
}

export default function HomePage() {
  const [date, setDate] = useState<Date | undefined>(new Date())
  const [salesToday, setSalesToday] = useState(0)
  const [salesIn7Days, setSalesIn7Days] = useState(0)

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
    fetch(`${import.meta.env.VITE_API_BASE_URL}/Stat/salesToday`)
      .then((res) => res.json())
      .then((data) => setSalesToday(data))
  }, [])

  useEffect(() => {
    fetch(`${import.meta.env.VITE_API_BASE_URL}/Stat/salesInLast7Days`)
      .then((res) => res.json())
      .then((data: SalesDay[]) => {
        const total = data.reduce((sum, item) => sum + item.transactions, 0)
        setSalesIn7Days(total)
      })
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
            <Calendar
              mode="single"
              selected={date}
              onSelect={setDate}
              className="rounded-lg border"
            />
          </div>

          <div className="flex flex-col gap-4 items-start">
            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md">
              <h2 className="text-lg text-gray-500">Sales Today</h2>
              <p className="mt-2 text-3xl font-bold">{salesToday}</p>
            </div>

            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md">
              <h2 className="text-lg text-gray-500">Sales Last Week</h2>
              <p className="mt-2 text-3xl font-bold">{salesIn7Days}</p>
            </div>
          </div>
        </div>

        <div className="mt-8">
          <h1>
            Today is: {today.toDateString()}
            <br />
            {today.getDate()}
          </h1>
        </div>

      <div className="mt-10 border-white border-2">
        <h1>Gacha Statistics</h1>
         <div className="flex flex-col gap-4 items-start">
            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md">
              <h2 className="text-lg text-gray-500">Gacha Statistics</h2>
              <p className="mt-2 text-3xl font-bold">{salesToday}</p>
            </div>
          </div>

          <div className="flex flex-col gap-4 items-start">
            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md">
              <h2 className="text-lg text-gray-500">Gacha Statistics</h2>
              <p className="mt-2 text-3xl font-bold">{salesToday}</p>
            </div>
          </div>
      </div>

       <div className="mt-10 border-white border-2">
        <h1>Menu Statistics</h1>
         <div className="flex flex-col gap-4 items-start">
            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md">
              <h2 className="text-lg text-gray-500">Gacha Statistics</h2>
              <p className="mt-2 text-3xl font-bold">{salesToday}</p>
            </div>
          </div>

          <div className="flex flex-col gap-4 items-start">
            <div className="w-60 rounded-2xl border-2 border-white p-6 text-center shadow-md">
              <h2 className="text-lg text-gray-500">Gacha Statistics</h2>
              <p className="mt-2 text-3xl font-bold">{salesToday}</p>
            </div>
          </div>
      </div>

        <SidebarTrigger className="absolute bottom-5 ml-auto h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}