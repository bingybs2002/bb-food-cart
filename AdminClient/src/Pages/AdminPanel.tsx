import { useEffect, useState } from "react"
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

const API = import.meta.env.VITE_API_BASE_URL ?? ""
const buildApiUrl = (path: string) => `${API}${path}`

type PopularItemtype = {
  name: string
  calories: number
  carbs: number
  protein: number
  totalSold: number
}

type SoldOutItemType = {
  id: number
  name: string
  allergies: string
  description: string
  calories: number
  carbs: number
  protein: number
}

type WeeklyStatEntry = {
  Date?: string
  date?: string
  Count?: number
  count?: number
}

export default function HomePage() {
  const [date, setDate] = useState<Date | undefined>(new Date())
  const [salesToday, setSalesToday] = useState(0)
  const [salesIn7Days, setSalesIn7Days] = useState(0)
  const [popularItem, setPopularItem] = useState<PopularItemtype | null>(null)
  const [SoldOutItems, setSoldOutItems] = useState<SoldOutItemType[]>([])
  const [chartData, setChartData] = useState<Array<{ month: string; Visits: number; Orders: number }>>([])

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

  useEffect(() => {
    const selectedDate = date ?? new Date()
    const startOfWeek = new Date(selectedDate)
    startOfWeek.setHours(0, 0, 0, 0)
    startOfWeek.setDate(startOfWeek.getDate() - startOfWeek.getDay())

    const weekDays = Array.from({ length: 7 }, (_, index) => {
      const day = new Date(startOfWeek)
      day.setDate(startOfWeek.getDate() + index)
      return day
    })

    const toDateKey = (value: string) => new Date(value).toISOString().slice(0, 10)

    const fetchWeeklyStats = async () => {
      try {
        const [salesResponse, visitsResponse] = await Promise.all([
          fetch(buildApiUrl(`/Stat/weeklySaleStat/${encodeURIComponent(selectedDate.toISOString())}`)),
          fetch(buildApiUrl(`/Stat/weeklyVisitStat/${encodeURIComponent(selectedDate.toISOString())}`)),
        ])

        if (!salesResponse.ok || !visitsResponse.ok) {
          throw new Error("Failed to load weekly statistics")
        }

        const [salesData, visitsData]: [WeeklyStatEntry[], WeeklyStatEntry[]] = await Promise.all([
          salesResponse.json(),
          visitsResponse.json(),
        ])

        const salesByDate = new Map<string, number>()
        const visitsByDate = new Map<string, number>()

        salesData.forEach((item) => {
          if (item.Date || item.date) {
            const key = toDateKey(item.Date ?? item.date ?? "")
            salesByDate.set(key, item.Count ?? item.count ?? 0)
          }
        })

        visitsData.forEach((item) => {
          if (item.Date || item.date) {
            const key = toDateKey(item.Date ?? item.date ?? "")
            visitsByDate.set(key, item.Count ?? item.count ?? 0)
          }
        })

        setChartData(
          weekDays.map((day) => ({
            month: day.toLocaleDateString("en-US", { weekday: "short" }),
            Visits: visitsByDate.get(toDateKey(day.toISOString())) ?? 0,
            Orders: salesByDate.get(toDateKey(day.toISOString())) ?? 0,
          }))
        )
      } catch (error) {
        console.error(error)
        setChartData(
          weekDays.map((day) => ({
            month: day.toLocaleDateString("en-US", { weekday: "short" }),
            Visits: 0,
            Orders: 0,
          }))
        )
      }
    }

    void fetchWeeklyStats()
  }, [date])

  useEffect(() => {
    fetch(buildApiUrl("/Stat/salesToday"))
      .then((res) => res.json())
      .then((data) => setSalesToday(data))
  }, [])

  useEffect(() => {
    fetch(buildApiUrl("/Stat/salesInLast7Days"))
      .then((res) => res.json())
      .then((data) => setSalesIn7Days(data))
  }, [])

  useEffect(() => {
    fetch(buildApiUrl("/Stat/MostPopularItem"))
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch most popular item")
        return res.json()
      })
      .then((data) => setPopularItem(data))
      .catch((err) => console.error(err))
  }, [])

  useEffect(() => {
    fetch(buildApiUrl("/Stat/SoldOutItems"))
      .then((res) => res.json())
      .then((data) => setSoldOutItems(data))
  }, [])

  return (
    <SidebarProvider>
      <AppSidebar/>

      <main className="relative w-full space-y-6 p-6 pb-24">
        <h1 className="mt-4 text-center text-3xl font-bold lg:text-4xl">
          Admin Dashboard | BB Food Cart
        </h1>

        <div className="flex justify-end">
          <div className="w-full max-w-xs rounded-xl border border-red-500 p-4">
            <h3 className="text-center font-semibold">Our Cart Address</h3>
            <p className="mt-2 text-right text-sm leading-6 text-stone-100">
              7350 Bustleton Avenue,
              <br />
              Philadelphia, 19149
            </p>
          </div>
        </div>

        <section>
          <h6 className="text-center text-lg font-bold text-heading">
            Sales & Statistics
          </h6>

          <div className="mt-6 grid grid-cols-1 gap-6 lg:grid-cols-2 lg:items-stretch">
            <div className="rounded-xl border-2 border-white p-6">
              <p className="mb-4 text-center font-semibold">Week Performance</p>

              <ChartContainer config={chartConfig} className="h-[320px] w-full">
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
              <p className="mt-3 text-center text-sm text-gray-400">
                Showing weekly visits and orders from the backend for the selected week.
              </p>
            </div>

            <div className="flex flex-col gap-6">
              <div className="flex min-h-[320px] items-center justify-center rounded-xl border-2 border-white p-4">
                <div>
                  <h1 className="text-center text-base font-medium">
                    {today.toDateString()}
                  </h1>
                  <Calendar
                    mode="single"
                    selected={date}
                    onSelect={setDate}
                    className="mt-3 rounded-lg border"
                  />
                </div>
              </div>

              <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                <div className="rounded-2xl border-2 border-white p-6 text-center shadow-md">
                  <h2 className="text-sm text-gray-400">Sales Today</h2>
                  <p className="mt-2 text-3xl font-bold">{salesToday}</p>
                </div>

                <div className="rounded-2xl border-2 border-white p-6 text-center shadow-md">
                  <h2 className="text-sm text-gray-400">Sales Last Week</h2>
                  <p className="mt-2 text-3xl font-bold">{salesIn7Days}</p>
                </div>
              </div>
            </div>
          </div>
        </section>

        <section>
          <h6 className="text-center text-lg font-bold text-heading">
            Menu and Items
          </h6>

          <div className="mt-6 grid grid-cols-1 gap-6 lg:grid-cols-2">
            <div className="rounded-xl border-2 border-white p-4">
              <p className="mb-4 text-center font-semibold">Sold Out Items</p>

              {SoldOutItems.length > 0 ? (
                <div className="space-y-3">
                  {SoldOutItems.map((item) => (
                    <div key={item.id} className="rounded-lg border border-white/20 p-3">
                      <p className="font-medium">{item.name}</p>
                      <p className="text-sm text-gray-400">
                        Number of sold out items: {SoldOutItems.length}
                      </p>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-center text-sm text-gray-400">No sold out items</p>
              )}
            </div>

            <div className="rounded-xl border-2 border-white p-4">
              <p className="mb-4 text-center font-semibold">Most Popular Items</p>

              {popularItem ? (
                <div className="space-y-2 rounded-lg border border-white/20 p-4">
                  <p><span className="font-medium">Name:</span> {popularItem.name}</p>
                  <p><span className="font-medium">Total Sold:</span> {popularItem.totalSold}</p>
                  <p><span className="font-medium">Calories:</span> {popularItem.calories}</p>
                  <p><span className="font-medium">Carbs:</span> {popularItem.carbs}</p>
                  <p><span className="font-medium">Protein:</span> {popularItem.protein}</p>
                </div>
              ) : (
                <p className="text-center text-sm text-gray-400">No data</p>
              )}
            </div>
          </div>
        </section>

        <SidebarTrigger className="absolute bottom-5 left-6 h-10 w-40 border-2 border-white" />
      </main>
    </SidebarProvider>
  )
}
