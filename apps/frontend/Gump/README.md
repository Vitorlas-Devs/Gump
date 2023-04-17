# Omega Turbo Nuxt App

## Nuxt Development

Nuxt maintains an inner Vue app in the `.nuxt` directory. For auto-imports and other features to work, you must run `npm run dev` with **Ctrl + Shift + B** and **Dev** selected.

A development server will be started at `localhost:3000`.

Click on the small Nuxt icon down in the center to launch the Nuxt DevTools.

## Resources

- [Nuxt folder structure](https://nuxt.com/docs/guide/directory-structure/nuxt)
- [UnoCSS Docs](https://unocss.dev/interactive/)
- [Icons to find](https://icones.netlify.app/collection/fa6-solid)
- [Capacitor Docs](https://ionic.nuxtjs.org/getting-started/#run-on-ios-or-android) (or read next paragraph)

## Deploy to Capacitor

To preview the app on Android, run `npm run build` with **Ctrl + Shift + B** and **Build** selected.

The build script does these things:

This will create a production build in `.output` and `dist` (`.output/public` symbolc link)

  ```bash
  npx nuxi generate
  ```

Next we sync the `dist` to Capacitor.
  
  ```bash
  npx cap sync
  ```

Finally, we run the android emulator.

  ```bash
  npx cap run android
  ```
