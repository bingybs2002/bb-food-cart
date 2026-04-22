-- Seeds one busy week of checked-out shopping cart transactions.
-- Run from psql while connected to your database:
--   \i 'C:/Users/Bing/Projects/bb-food-cart/Backend/Testing/Miscellaneous/busy_week_transactions.sql'
--
-- This script uses existing rows from "Customers" and "Foods".
-- It does not insert customers, menu items, nutrition, or auth users.

DO $$
DECLARE
    start_day date := (CURRENT_DATE - INTERVAL '6 days')::date;
    day_offset int;
    order_index int;
    item_index int;
    orders_for_day int;
    items_for_order int;
    selected_customer_id int;
    selected_food_id int;
    selected_quantity int;
    new_cart_id int;
    cart_created_at timestamptz;
    order_counts int[] := ARRAY[28, 34, 47, 63, 82, 96, 74];
BEGIN
    IF NOT EXISTS (SELECT 1 FROM "Customers") THEN
        RAISE EXCEPTION 'Cannot seed busy week: "Customers" has no rows.';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM "Foods") THEN
        RAISE EXCEPTION 'Cannot seed busy week: "Foods" has no rows.';
    END IF;

    FOR day_offset IN 0..6 LOOP
        orders_for_day := order_counts[day_offset + 1];

        FOR order_index IN 1..orders_for_day LOOP
            SELECT "Id"
            INTO selected_customer_id
            FROM "Customers"
            ORDER BY random()
            LIMIT 1;

            cart_created_at :=
                (start_day + day_offset)
                + make_interval(hours => floor(random() * 14)::int + 8)
                + make_interval(mins => floor(random() * 60)::int)
                + make_interval(secs => floor(random() * 60)::int);

            INSERT INTO "ShoppingCarts"
                ("CustomerId", "IsCheckedOut", "CreatedDate", "IsCancelled")
            VALUES
                (selected_customer_id, true, cart_created_at, false)
            RETURNING "Id" INTO new_cart_id;

            items_for_order := floor(random() * 4)::int + 1;

            FOR item_index IN 1..items_for_order LOOP
                SELECT "Id"
                INTO selected_food_id
                FROM "Foods"
                ORDER BY random()
                LIMIT 1;

                selected_quantity := floor(random() * 4)::int + 1;

                INSERT INTO "CartItems"
                    ("ShoppingCartId", "FoodId", "Quantity", "UnitPrice", "CreateDate")
                VALUES
                    (new_cart_id, selected_food_id, selected_quantity, 0, cart_created_at);
            END LOOP;
        END LOOP;
    END LOOP;
END $$;
