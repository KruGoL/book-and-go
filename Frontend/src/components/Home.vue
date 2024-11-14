<template>
  <transition name="load" appear>
    <div class="q-pa-md">
      <q-carousel
        animated
        v-model="slide"
        navigation
        infinite
        :autoplay="autoplay"
        arrows
        transition-prev="slide-right"
        transition-next="slide-left"
        @mouseenter="autoplay = false"
        @mouseleave="autoplay = true"
      >
        <q-carousel-slide
          v-for="(picture, i) in pictures"
          :name="i"
          :img-src=picture.src
        />
      </q-carousel>
    </div>
  </transition>  

  <transition name="load" appear>
    <div>
      <h3 class="text-center" >
        <strong>About Us</strong>
      </h3>
      <hr class="mt-5"/>
      <p class="mt-5 text-center font-semibold">
        Book-n-Go is a convenient toolkit for managing any bookable items in a form of a web application.<br>

        Our application is user-friendly, so it doesn’t require any special skills.<br>

        Since it is a web application, clients can easily integrate it with their
        businesses.<br>

        The main functionality is visual booking, which greatly improves user
        experience.
      </p>
    </div>
  </transition>  
</template>

<script lang="ts">
import { ref } from 'vue';
export default {
  data() {
    return {
      pictures: [
        {
          src: 'https://images.wallpaperscraft.com/image/single/restaurant_tables_chairs_comfort_interior_39213_1600x900.jpg',
        },
        {
          src: 'https://static.visitestonia.com/images/3800472/1600_900_false_false_02eebb2a448478e69b1ed9b505749070.jpg',
        },
        {
          src: 'https://images.wallpaperscraft.ru/image/single/restoran_banket_oformlenie_stoly_stulya_dizayn_39238_1600x900.jpg',
        },
      ],
    };
  },
  setup() {
    return {
      slide: ref(1),
      autoplay: ref(true),
    };
  },
};
</script>

<style>
  .load-enter-from{
    opacity: 0.8;
    transform: scale(0.98);
  }
  .load-enter-to{
    opacity: 1;
    transform: scale(1);
  }
  .load-enter-active{
    transition: all 1s ease;
  }
</style>
