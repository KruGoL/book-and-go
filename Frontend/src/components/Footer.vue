<template>
    <v-container>
    <v-footer
    dark
    padless
  >
    <v-card
      flat
      tile
      width="100%"
      class="text-center"
    >
      <v-card-text>
        <v-btn
          v-for="icon in icons"
          :key="icon"
          class="mx-4 white--text"
          icon
        >
          <v-icon size="24px">
            {{ icon }}
          </v-icon>
        </v-btn>
      </v-card-text>

      <v-card-text class="white--text">
        Phasellus feugiat arcu sapien, et iaculis ipsum elementum sit amet.
      </v-card-text>

      <v-divider></v-divider>

      <v-card-text class="white--text">
        {{ new Date().getFullYear() }} — <strong>Book-n Go</strong>
      </v-card-text>
    </v-card>
  </v-footer>
</v-container>
</template>

<script lang='ts'>
export default {
    name: "Footer",

    data: () => ({
    icons: [
        'mdi-facebook',
        'mdi-twitter',
        'mdi-linkedin',
        'mdi-instagram',
        ],
    }),
}
</script>

<style scoped>
    footer {
        background-color:lightskyblue;
        padding: 0px;
    }
</style>