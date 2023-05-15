import type { DebouncedFunc } from 'lodash-es'
import { debounce } from 'lodash-es'

// Define a variable to store the debounced function and the changed recipe
let debouncedRecipe: { debouncedUpdate: DebouncedFunc<(changed: Partial<Recipe>) => void>; changedRecipe: Partial<Recipe> } | undefined

export function debouncedRecipeUpdate<T extends Recipe>(recipe: T): T {
  const recipeStore = useRecipeStore()

  // Check if the debounced recipe is undefined or has a different id than the current recipe
  if (!debouncedRecipe || debouncedRecipe.changedRecipe.id !== recipe.id) {
    // Create a new debounced recipe with a new debounced function and a new changed recipe
    debouncedRecipe = {
      debouncedUpdate: debounce((changed: Partial<T>) => {
        recipeStore.updateRecipe(recipe.id, changed)
      }, 1500),
      changedRecipe: { ...recipe },
    }
  }

  // Loop over the recipe properties
  for (const prop in recipe) {
    const key = prop as keyof Recipe

    // Check if there is a change in the property value
    if (debouncedRecipe && recipe[key] !== debouncedRecipe.changedRecipe[key]) {
      // Update the changed recipe with the new value
      setValues(debouncedRecipe.changedRecipe, key, recipe[key])
      // Call the debounced function with the changed recipe
      debouncedRecipe.debouncedUpdate(debouncedRecipe.changedRecipe)
    }
  }

  return recipe
}
