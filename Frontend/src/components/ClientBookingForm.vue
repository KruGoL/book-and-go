<template>
  <q-form class="ml-5 mr-5 text-center">
    <div class="mt-7 mb-3">
      <text class="font-semibold text-25px text-dark-400">Booking Form</text>
    </div>
    <q-separator rounded color="dark" class="mr-2 ml-2" />

    <q-input
      class="mt-5"
      clearable
      clear-icon="clear"
      type="text"
      v-model="booking.objectId"
      label="Table"
      placeholder="Any"
      readonly
    />
    <q-input
      class="mt-5"
      clearable
      clear-icon="clear"
      type="text"
      v-model="booking.name"
      label="Name"
      lazy-rules
      :rules="[(val) => (val && val.length > 0) || 'Please type your name']"
    />
    <q-input
      class="mt-5"
      clearable
      clear-icon="clear"
      type="email"
      v-model="booking.email"
      label="Email"
    />
    <q-input
      class="mt-5"
      clearable
      clear-icon="clear"
      type="text"
      v-model="booking.phoneNumber"
      label="Phone Number"
      lazy-rules
      :rules="[
        (val) => (val && val.length > 0) || 'Please type your phonenumber',
      ]"
    />
    <q-input class="mt-3" v-model="updatedDate">
      <template v-slot:append>
        <q-icon name="event" class="cursor-pointer">
          <q-popup-proxy cover transition-show="scale" transition-hide="scale">
            <q-date v-model="updatedDate" mask="HH:mm DD MMMM YYYY">
              <div class="row items-center justify-end">
                <q-btn v-close-popup label="Close" color="primary" flat />
              </div>
            </q-date>
          </q-popup-proxy>
        </q-icon>
      </template>
      <template v-slot:prepend>
        <q-icon name="access_time" class="cursor-pointer">
          <q-popup-proxy cover transition-show="scale" transition-hide="scale">
            <q-time v-model="updatedDate" mask="HH:mm DD MMMM YYYY" format24h>
              <div class="row items-center justify-end">
                <q-btn v-close-popup label="Close" color="primary" flat />
              </div>
            </q-time>
          </q-popup-proxy>
        </q-icon>
      </template>
    </q-input>
    <q-btn
      class="send"
      label="Send"
      @click.prevent="addNewBooking"
      type="submit"
      color="primary"
    />
    <q-btn
      flat
      class="reset"
      label="Clear"
      @click.prevent="resetBooking"
      type="reset"
      color="primary"
    />
  </q-form>
  {{ booking }}
</template>

<script lang="ts" setup>
import { Booking } from '@/model/booking';
import { useBookingsStore } from '@/stores/bookingStore';
import {
  date,
  QForm,
  QSeparator,
  QInput,
  QIcon,
  QPopupProxy,
  QDate,
  QBtn,
  QTime,
} from 'quasar';
import { Ref, ref, watch } from 'vue';
import { v4 as uuidv4 } from 'uuid';
import router from '@/router';

const props = defineProps<{
  selectedId: string | undefined;
  facilityId: string
}>();

const { addBooking } = useBookingsStore();
const booking: Ref<Booking> = ref<Booking>({});

const updatedDate = ref(
  date.formatDate(
    date.addToDate(new Date(), { days: 1 }),
    'HH:mm DD MMMM YYYY',
  ),
);

const addNewBooking = () => {
  if (
    booking.value.name == '' ||
    booking.value.phoneNumber == ''
  )
    return;
  booking.value.id = uuidv4()
  booking.value.facilityId = props.facilityId
  addBooking(booking.value);

  router.push({ name: 'Thanks Page', params: { id: booking.value.id } });
};

const resetBooking = () => {
  const current = booking.value;
  current.email = '';
  current.name = '';
  current.objectId = '';
  current.phoneNumber = '';
};
watch(
  () => props.selectedId,
  () => {
    booking.value.objectId = props.selectedId;
  },
);

watch(
  updatedDate,
  () => {
    const dParse = Date.parse(updatedDate.value);
    booking.value.bookingDate = new Date(dParse);
  },
);
</script>

<style lang="scss" scoped>
.send {
  margin-top: 10%;
  border-radius: 10px;
  display: flex;
  height: 100%;
  width: 100%;
}

.reset {
  margin-top: 5%;
  border-radius: 10px;
  display: flex;
  height: 100%;
  width: 100%;
}
</style>
