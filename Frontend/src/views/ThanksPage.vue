<template>
    <div class="main-page">
      <q-card-section class="text-center">
        <text class="font-bold text-50px">Thank you,! </text>
        <div class="mt-5" mask="HH:mm" >
          <text class="font-medium">Your booking date:</text> 
          {{bDate}} </div>
        <div class="text-subtitle2" mask="HH:mm" >Booking time : {{bTime}} </div>
      </q-card-section>
    </div>
    <router-link class="go-back" :to="routeBack" ><q-btn label = "Back to booking"></q-btn></router-link>
    {{routeBack}}
    {{booking}}
</template>

<script lang="ts" setup>
import { Booking } from '@/model/booking';
import { useBookingsStore } from '@/stores/bookingStore';
import { date,QCardSection} from 'quasar';
import { onMounted, ref, Ref } from 'vue';
import { useRoute } from 'vue-router';
import { storeToRefs } from 'pinia';

const route = useRoute();

const bookingsStore = useBookingsStore();
const { booking } = storeToRefs(bookingsStore);

const bookingThis: Ref<Booking> = ref<Booking>({});
const bookingId: Ref<string> = ref<string>('');

const routeBack= ref('')
const bDate = ref('')
const bTime= ref('')
onMounted(async() => {
  bookingId.value = String(useRoute().params.id);
  await bookingsStore.GETById(bookingId.value);
  bookingThis.value = booking.value!

  routeBack.value = '/client/' + String(route.params.id)
  bDate.value = date.formatDate(bookingThis.value.bookingDate, 'YYYY-MMMM-DD')
  bTime.value = date.formatDate(bookingThis.value.bookingDate, 'HH:mm:ss')
});

</script>

<style>
.main-page{
  display: flex;
  align-items:center;
  justify-content: center;
  margin-top: 10%;
}
.go-back{
  display: flex;
  align-items:center;
  justify-content: center;
}
</style>
