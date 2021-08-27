<template>
    <component :is="window" :data="data"></component>
</template>

<script>
import Components from './components'

import Bank from './windows/Bank.vue'
import Bann from './windows/Bann.vue'
import Barber from './windows/Barber.vue'
import BuyCarShop from './windows/BuyCarShop.vue'
import CannabisLabor from './windows/CannabisLabor.vue'
import CharacterCreator from './windows/CharacterCreator.vue'
import Chat from './windows/Chat.vue'
import ClothingShop from './windows/ClothingShop.vue'
import Confirmation from './windows/Confirmation.vue'
import Death from './windows/Death.vue'
import Equipment from './windows/Equipment.vue'
import Flyer from './windows/Flyer'
import Frisk from './windows/Frisk.vue'
import Fullscreen from './windows/Fullscreen.vue'
import Garage from './windows/Garage.vue'
import GiveMoney from './windows/GiveMoney.vue'
import HouseMenue from './windows/HouseMenue.vue'
import Inventory from './windows/Inventory.vue'
import Keys from './windows/Keys.vue'
import Kick from './windows/Kick.vue'
import Login from './windows/Login.vue'
import MethLabor from './windows/MethLabor.vue'
import Register from './windows/Register.vue'
import RouletteTable from './windows/RouletteTable.vue'
import Shop from './windows/Shop.vue'
import SlotMachine from './windows/SlotMachine.vue'
import TattooShop from './windows/TattooShop.vue'
import TattooLicenseShop from './windows/TattooLicenseShop.vue'
import TextInputBox from './windows/TextInputBox.vue'
import Insurance from './windows/Insurance.vue'
import Workstation from './windows/Workstation.vue'
import VehicleRent from './windows/VehicleRent.vue'
import EjectWindow from './windows/EjectWindow.vue'
import Wardrobe from './windows/Wardrobe'
import AnimationWheelFavoritesList from './windows/AnimationWheelFavoritesList'

export default Components.register({
    name: 'Windows',

    data: function () {
        return {
            window: '',
            data  : {},
        }
    },

    methods: {
        show: function (window, data = null) {
            this.data   = data
            this.window = window
        },
    },

    components: {
        Bank,
        Bann,
        Barber,
        BuyCarShop,
        CannabisLabor,
        CharacterCreator,
        Chat,
        ClothingShop,
        Confirmation,
        Death,
        Equipment,
        Flyer,
        Frisk,
        Fullscreen,
        Garage,
        GiveMoney,
        HouseMenue,
        Inventory,
        Keys,
        Kick,
        Login,
        MethLabor,
        Register,
        RouletteTable,
        Shop,
        SlotMachine,
        TattooShop,
        TextInputBox,
        Insurance,
        Workstation,
        EjectWindow,
        VehicleRent,
        TattooLicenseShop,
        Wardrobe,
        AnimationWheelFavoritesList,
    },
})
</script>