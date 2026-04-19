"use client"

import * as React from "react"
import { Link } from "react-router-dom"

import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Field,
  FieldDescription,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import {
  Combobox,
  ComboboxChip,
  ComboboxChips,
  ComboboxChipsInput,
  ComboboxContent,
  ComboboxEmpty,
  ComboboxItem,
  ComboboxList,
  ComboboxValue,
} from "@/components/ui/combobox"

type RegisterFormData = {
  cosignee: string
  phoneNumber: string
  password: string
  confirmPassword: string
  streetAddress: string
  city: string
  state: string
  zipCode: string
}

type RegisterRequest = {
  phoneNumber: string
  password: string
  cosignee: string
  allergies: number
  streetAddress: string
  city: string
  state: string
  zipCode: string
}

const allergicItems = [
  "None",
  "Peanut",
  "TreeNuts",
  "Milk",
  "Egg",
  "Soy",
  "Wheat",
  "Fish",
  "ShellFish",
  "Sesame",
]

const allergyMap: Record<string, number> = {
  None: 0,
  Peanut: 1 << 0,
  TreeNuts: 1 << 1,
  Milk: 1 << 2,
  Egg: 1 << 3,
  Soy: 1 << 4,
  Wheat: 1 << 5,
  Fish: 1 << 6,
  ShellFish: 1 << 7,
  Sesame: 1 << 8,
}

export function SignupForm({
  className,
  ...props
}: React.ComponentProps<"form">) {
  const [allergies, setAllergies] = React.useState<string[]>(["None"])

  const [form, setForm] = React.useState<RegisterFormData>({
    cosignee: "",
    phoneNumber: "",
    password: "",
    confirmPassword: "",
    streetAddress: "",
    city: "",
    state: "",
    zipCode: "",
  })

  const [error, setError] = React.useState("")
  const [success, setSuccess] = React.useState("")
  const [isLoading, setIsLoading] = React.useState(false)

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id, value } = e.target

    setForm((prev) => ({
      ...prev,
      [id]: value,
    }))
  }

  const handleAllergyChange = (values: string[]) => {
    if (values.length === 0) {
      setAllergies(["None"])
      return
    }

    if (values.includes("None") && values.length > 1) {
      setAllergies(values.filter((v) => v !== "None"))
      return
    }

    setAllergies(values)
  }

  const getAllergyFlag = (selected: string[]) => {
    if (selected.length === 0 || selected.includes("None")) {
      return 0
    }

    return selected.reduce((acc, item) => {
      return acc | (allergyMap[item] ?? 0)
    }, 0)
  }

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    setError("")
    setSuccess("")

    if (form.password !== form.confirmPassword) {
      setError("Passwords do not match.")
      return
    }

    const payload: RegisterRequest = {
      phoneNumber: form.phoneNumber,
      password: form.password,
      cosignee: form.cosignee,
      allergies: getAllergyFlag(allergies),
      streetAddress: form.streetAddress,
      city: form.city,
      state: form.state,
      zipCode: form.zipCode,
    }

    try {
      setIsLoading(true)

      const res = await fetch(
        `${import.meta.env.VITE_API_BASE_URL}/account/register`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        }
      )

      if (!res.ok) {
        const message = await res.text()
        setError(message || "Registration failed.")
        return
      }

      setSuccess("Account created successfully.")

      setForm({
        cosignee: "",
        phoneNumber: "",
        password: "",
        confirmPassword: "",
        streetAddress: "",
        city: "",
        state: "",
        zipCode: "",
      })

      setAllergies(["None"])
    } catch {
      setError("Network error. Please try again.")
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <form
      onSubmit={handleSubmit}
      className={cn("flex flex-col gap-6", className)}
      {...props}
    >
      <FieldGroup>
        <div className="flex flex-col items-center gap-1 text-center">
          <h1 className="text-2xl font-bold">Create your account</h1>
          <p className="text-sm text-balance text-muted-foreground">
            Fill in the form below to create your account.
          </p>
        </div>

        <Field>
          <FieldLabel htmlFor="cosignee">Full Name</FieldLabel>
          <Input
            id="cosignee"
            type="text"
            placeholder="John Doe"
            required
            className="bg-background"
            value={form.cosignee}
            onChange={handleChange}
          />
        </Field>

        <Field>
          <FieldLabel htmlFor="phoneNumber">Phone</FieldLabel>
          <Input
            id="phoneNumber"
            type="tel"
            placeholder="xxx-xxx-xxxx"
            required
            className="bg-background"
            value={form.phoneNumber}
            onChange={handleChange}
          />
          <FieldDescription>
            We will not share your information with anyone else. Your data will
            be kept secure with us.
          </FieldDescription>
        </Field>

        <Field>
          <FieldLabel htmlFor="password">Password</FieldLabel>
          <Input
            id="password"
            type="password"
            required
            className="bg-background"
            value={form.password}
            onChange={handleChange}
          />
          <FieldDescription>
            Must be at least 4 characters long.
          </FieldDescription>
        </Field>

        <Field>
          <FieldLabel htmlFor="confirmPassword">Confirm Password</FieldLabel>
          <Input
            id="confirmPassword"
            type="password"
            required
            className="bg-background"
            value={form.confirmPassword}
            onChange={handleChange}
          />
          <FieldDescription>Please confirm your password.</FieldDescription>
        </Field>

        <Field>
          <FieldLabel htmlFor="allergies">Allergies</FieldLabel>

          <Combobox
            multiple
            autoHighlight
            items={allergicItems}
            value={allergies}
            onValueChange={handleAllergyChange}
          >
            <ComboboxChips className="w-full">
              <ComboboxValue>
                {(values) => (
                  <>
                    {values.map((value: string) => (
                      <ComboboxChip key={value}>{value}</ComboboxChip>
                    ))}
                    <ComboboxChipsInput
                      id="allergies"
                      placeholder="Select allergies..."
                    />
                  </>
                )}
              </ComboboxValue>
            </ComboboxChips>

            <ComboboxContent>
              <ComboboxEmpty>No items found.</ComboboxEmpty>
              <ComboboxList>
                {(item) => (
                  <ComboboxItem key={item} value={item}>
                    {item}
                  </ComboboxItem>
                )}
              </ComboboxList>
            </ComboboxContent>
          </Combobox>
        </Field>

        <Field>
          <FieldLabel htmlFor="streetAddress">Street Address</FieldLabel>
          <Input
            id="streetAddress"
            type="text"
            placeholder="123 Main St"
            required
            className="bg-background"
            value={form.streetAddress}
            onChange={handleChange}
          />
        </Field>

        <Field>
          <FieldLabel htmlFor="city">City</FieldLabel>
          <Input
            id="city"
            type="text"
            placeholder="City"
            className="bg-background"
            value={form.city}
            onChange={handleChange}
          />
        </Field>

        <div className="grid grid-cols-3 gap-4">
          <Field>
            <FieldLabel htmlFor="state">State</FieldLabel>
            <Input
              id="state"
              type="text"
              placeholder="State"
              className="bg-background"
              value={form.state}
              onChange={handleChange}
            />
          </Field>

          <Field className="col-span-2">
            <FieldLabel htmlFor="zipCode">Zip Code</FieldLabel>
            <Input
              id="zipCode"
              type="text"
              placeholder="12345"
              className="bg-background"
              value={form.zipCode}
              onChange={handleChange}
            />
          </Field>
        </div>

        {error && (
          <p className="text-sm text-red-500">{error}</p>
        )}

        {success && (
          <p className="text-sm text-green-600">{success}</p>
        )}

        <Field>
          <Button type="submit" className="w-full" disabled={isLoading}>
            {isLoading ? "Creating Account..." : "Create Account"}
          </Button>
        </Field>

        <Field>
          <FieldDescription className="px-6 text-center">
            Already have an account?{" "}
            <Link to="/login" className="underline-offset-4 hover:underline">
              Sign in
            </Link>
          </FieldDescription>
        </Field>
      </FieldGroup>
    </form>
  )
}